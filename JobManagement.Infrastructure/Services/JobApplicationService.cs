using AutoMapper;
using JobManagement.Application.Contracts.Email;
using JobManagement.Application.Contracts.Persistence;
using JobManagement.Application.Contracts.Services;
using JobManagement.Application.DTOs.Common;
using JobManagement.Application.DTOs.JobApplication;
using JobManagement.Application.Enums;
using JobManagement.Application.Exceptions;
using JobManagement.Application.Models.Email;
using JobManagement.Application.Validations.JobApplication;
using JobManagement.Application.Validations.Sort;
using JobManagement.Domain;
using JobManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace JobManagement.Infrastructure.Services
{
    public class JobApplicationService : GenericService, IJobApplicationService
    {
        private readonly IMapper _mapper;
        private readonly IJobApplicationRepository _jobApplicationRepository;      
        private readonly IJobRepository _jobRepository;      
        private readonly IFileStorageService _fileStorageService;
        private readonly IEmailSender _emailSender;

        public JobApplicationService(IJobApplicationRepository jobApplicationRepository, IMapper mapper,
                IJobRepository jobRepository,
                IFileStorageService fileStorageService,
                IEmailSender emailSender,
                UserManager<ApplicationUser> userManager,
                CurrentUserService currentUserService) : base(userManager, currentUserService)
        {
            this._mapper = mapper;
            this._jobApplicationRepository = jobApplicationRepository;       
            this._jobRepository = jobRepository;
            this._fileStorageService = fileStorageService;
            this._emailSender = emailSender;
        }


        //Retrieve the paginated list of all database entities
        public async Task<List<JobApplicationDto>> GetPagedAsync(PaginationDto dto)
        {
            var jobApplications = await _jobApplicationRepository.GetPagedAsync(dto.PageNumber, dto.PageSize);
            var data = _mapper.Map<List<JobApplicationDto>>(jobApplications);
            return data;
        }



        //Retrieve all applications submitted by a particular student using user Id
        public async Task<List<JobApplicationDto>> GetByUserAsync(PaginationDto dto)
        {
            bool isStudent = await GetUserRole("Student");

            if (!isStudent)
            {
                throw new ForbiddenException("Only students can view their own job applications.");
            }


            var jobApplications = await _jobApplicationRepository
                    .GetByUserIdAsync(
                        _currentUserService.UserId, 
                        dto.PageNumber, 
                        dto.PageSize);

            var data = _mapper.Map<List<JobApplicationDto>>(jobApplications);
            return data;

        }



        //Retrieve all applications submitted for a articular job using job Id
        //Ensures only users associated with the job's company can view its applications
        public async Task<List<JobApplicationDto>> GetByJobAsync(int jobId, PaginationDto dto)
        {
            var job = await _jobRepository.GetByIdAsync(jobId);

            if (job == null)
            {
                throw new NotFoundException(nameof(Job), jobId);
            }


            string exceptionMessage = "You can only view job applications submitted for jobs posted by your own company.";
            await CheckUserAuthorization(job.CompanyId, exceptionMessage);


            var jobApplications = await _jobApplicationRepository.GetByJobIdAsync(jobId, dto.PageNumber, dto.PageSize);
            var data = _mapper.Map<List<JobApplicationDto>>(jobApplications);
            return data;

        }



        //Retrieve a single job application entity using the Id
        public async Task<JobApplicationDto> GetByIdAsync(int id)
        {
            var jobApplication = await _jobApplicationRepository.GetByIdAsync(id);

            if (jobApplication == null)
            {
                throw new NotFoundException(nameof(JobApplication), id);
            }


            var data = _mapper.Map<JobApplicationDto>(jobApplication);
            return data;
        }



        //Creates a new job application entity
        //Ensures only a user with "Student" role can apply for a job
        public async Task<int> CreateAsync(CreateJobApplicationDto dto)
        {
            //Check if the applicant is a student
            bool isStudent = await GetUserRole("Student");

            if (!isStudent)
            {
                throw new ForbiddenException("Only students can apply for this job.");
            }


            //Check that the specified job exists before creating the application
            var job = await _jobRepository.GetByIdWithCompanyAsync(dto.JobId);

            if (job == null)
            {
                throw new NotFoundException(nameof(JobApplication), dto.JobId);
            }


            // Ensure the application deadline has not already passed
            if (job.ApplicationDeadline <= DateTime.Now)
            {
                throw new BadRequestException(
                    "You cannot apply for this job because its application deadline has passed.");
            }



            //Validate the dto
            var validator = new CreateJobApplicationValidator();
            var validationResult = await validator.ValidateAsync(dto);

            if (validationResult.Errors.Any())
            {
                throw new BadRequestException("Invalid job application entity. ", validationResult);
            }



            //Ensure that no other applicant has applied with the same email address for a job
            bool isEmailUnique = await _jobApplicationRepository.IsEmailUniqueAsync(dto.Email, dto.JobId);

            if (!isEmailUnique)
            {
                throw new ConflictException("An application with this email address already exists for this job.");
            }


            var jobApplication = _mapper.Map<JobApplication>(dto);
            jobApplication.DateApplied = DateTime.Now;
            jobApplication.ApplicantId = _currentUserService.UserId;


            //Save application document (CV/Resume) in Supabase
            if (dto.ApplicationDocument != null)
            {
                await using var stream = dto.ApplicationDocument.OpenReadStream();
                var key = await _fileStorageService.UploadAsync(
                    stream, 
                    dto.ApplicationDocument.FileName,
                    dto.ApplicationDocument.ContentType,
                    "JobApplication");
                jobApplication.ApplicationDocumentKey = key;
            }
                           

            await _jobApplicationRepository.CreateAsync(jobApplication);


            //Notify the user about successful submittion of their job application through email
            var user = await _userManager.FindByIdAsync(jobApplication.ApplicantId);

            if (user != null && !string.IsNullOrEmpty(user.Email))
            {
                await _emailSender.SendEmail(new EmailMessageData
                {
                    To = user.Email,
                    Subject = "Application Submitted Successfully",
                    Body = $"Hello {user.FirstName},\n\n" +
                   $"Your application for {job.Title} at {job.Company.Name} has been successfully submitted.\n" +
                   $"Date applied: {jobApplication.DateApplied}.\n" +
                   "Your application has been recorded in the Job Management System. You can log in to view your application and any future status updates.\n\n" +
                   "Thank you,\n" +
                   "Job Management System"
                });
            }

            return jobApplication.Id;
        }



        //Change the status of job application entity.
        //Only people associated with the Company of the posted job have the authority to perform this task
        public async Task ChangeStatus(ChangeJobApplicationStatusDto dto)
        {   
            var jobApplication = await _jobApplicationRepository.GetByIdAsync(dto.Id);

            if (jobApplication == null)
            {
                throw new NotFoundException(nameof(JobApplication), dto.Id);
            }


            //Fetch the job entity
            var job = await _jobRepository.GetByIdWithCompanyAsync(jobApplication.JobId);

            if (job == null)
            {
                throw new NotFoundException(nameof(Job), jobApplication.JobId);
            }


            //Ensure the company user owns the job  
            string exceptionMessage = "Only users associated with the company who posted the job can change the application status.";
            await CheckUserAuthorization(job.CompanyId ,exceptionMessage);


            //Validate the dto
            var validator = new ChangeJobApplicationStatusValidator();
            var validationResult = await validator.ValidateAsync(dto);

            if (validationResult.Errors.Any())
            {
                throw new BadRequestException("Invalid job application status. ", validationResult);
            }


            _mapper.Map(dto, jobApplication);
            await _jobApplicationRepository.UpdateAsync(jobApplication);


            //Notify the user about the status update of their job application through email

            var user = await _userManager.FindByIdAsync(jobApplication.ApplicantId);

            if (user != null && !string.IsNullOrEmpty(user.Email))
            {
                await _emailSender.SendEmail(new EmailMessageData
                {
                    To = user.Email,
                    Subject = "Application Status Updated",
                    Body = $"Hello {user.FirstName},\n\n" +
                   $"The status of your application for {job.Title} at {job.Company.Name} has been updated.\n" +
                   $"New status: {jobApplication.Status}.\n" +
                   "Please log in to the Job Management System to view your application details.\n\n" +
                   "Thank you,\n" +
                   "Job Management System"
                });
            }
        }



        //Deleted a job application entity
        public async Task DeleteAsync(int id)
        {
            var jobApplication = await _jobApplicationRepository.GetByIdAsync(id);

            if (jobApplication == null)
            {
                throw new NotFoundException(nameof(JobApplication), id);
            }


            //Fetch the job entity
            var job = await _jobRepository.GetByIdAsync(jobApplication.JobId);

            if (job == null)
            {
                throw new NotFoundException(nameof(Job), jobApplication.JobId);
            }


            //Ensure the company user owns the job  
            string exceptionMessage = "Only users associated with the company who posted the job can delete the submitted applications.";
            await CheckUserAuthorization(job.CompanyId, exceptionMessage);


            await _jobApplicationRepository.DeleteAsync(jobApplication);
        }



        // Retrieves entities that match the specified search criteria.
        public async Task<List<JobApplicationDto>> SearchAsync(SearchDto dto)
        {
            var searchTerm = dto.SearchTerm?.Trim();

            if (string.IsNullOrEmpty(searchTerm))
            {
                // Return all entities without filtering
                return await GetPagedAsync(new PaginationDto());
            }

            var jobApplications = await _jobApplicationRepository.SearchAsync(searchTerm);

            return _mapper.Map<List<JobApplicationDto>>(jobApplications);
        }




        // Retrieves entities sorted according to the specified sort option.
        public async Task<List<JobApplicationDto>> SortAsync(SortDto dto)
        {
            //Validate the dto
            var validator = new SortValidator();
            var validationResult = await validator.ValidateAsync(dto);

            if (validationResult.Errors.Any())
            {
                throw new BadRequestException("Invalid search term. ", validationResult);
            }


            IReadOnlyList<JobApplication> data;

            switch (dto.SortBy)
            {
                case SortOption.Ascending:
                    data = await _jobApplicationRepository.SortAsync(false, x => x.FirstName);
                    break;

                case SortOption.Descending:
                    data = await _jobApplicationRepository.SortAsync(true, x => x.FirstName);
                    break;

                case SortOption.Latest:
                    data = await _jobApplicationRepository.SortAsync(true, x => x.DateApplied);
                    break;

                case SortOption.Oldest:
                    data = await _jobApplicationRepository.SortAsync(false, x => x.DateApplied);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(
                    nameof(dto.SortBy),
                    dto.SortBy,
                    "Invalid sort option");
            }

            return _mapper.Map<List<JobApplicationDto>>(data);
        }    

    }
}
