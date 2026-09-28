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
using JobManagement.Domain.Enums;
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
                ICurrentUserService currentUserService) : base(userManager, currentUserService)
        {
            this._mapper = mapper;
            this._jobApplicationRepository = jobApplicationRepository;       
            this._jobRepository = jobRepository;
            this._fileStorageService = fileStorageService;
            this._emailSender = emailSender;
        }


        //Retrieve the paginated list of all database entities
        //Students can only view their own applications
        //Company users can view applications for the jobs posted by their own company
        public async Task<List<JobApplicationDto>> GetPagedAsync(PaginationDto dto)
        {
            IReadOnlyList<JobApplication> jobApplications = new List<JobApplication>();

            var userId = _currentUserService.UserId;
            bool isStudent = await GetUserRole("Student");
            bool isCompany = await GetUserRole("Company");

            if (isStudent)
            {
                jobApplications = await _jobApplicationRepository.GetByUserIdAsync(userId, dto.PageNumber, dto.PageSize);
            }

            else if (isCompany)
            {
                var companyId = await GetUserCompanyId();

                jobApplications = await _jobApplicationRepository.GetByCompanyIdAsync(companyId, dto.PageNumber, dto.PageSize);
            }


            else
            {
                throw new ForbiddenException("You are not authorized to view job applications.");
            }

            var data = _mapper.Map<List<JobApplicationDto>>(jobApplications);
            return data;
        }





        //Retrieves a single job application based on the user role
        //Students can only view their own applications
        //Company users can view applications for the jobs posted by their own company
        public async Task<JobApplicationDto> GetByIdAsync(int id)
        {
            var userId = _currentUserService.UserId;
            bool isStudent = await GetUserRole("Student");
            bool isCompany = await GetUserRole("Company");


            var jobApplication = await _jobApplicationRepository.GetByIdWithJobAsync(id);

            if (jobApplication == null)
            {
                throw new NotFoundException(nameof(JobApplication), id);
            }
          

            if (isStudent)
            {
                if (userId != jobApplication.ApplicantId)
                {
                    throw new ForbiddenException("You can only view your own job applications.");
                }      
            }

            else if (isCompany)
            {
                var companyId = await GetUserCompanyId();


                if (jobApplication.Job!.CompanyId != companyId)
                {
                    throw new ForbiddenException("You can only view job applications of your own company.");
                }
            }


            else
            {
                throw new ForbiddenException("You are not authorized to view job applications.");
            }


            var data = _mapper.Map<JobApplicationDto>(jobApplication);
            return data;
        }



        //Creates a new job application entity
        //Only a user with "Student" role can apply for a job
        public async Task<int> CreateAsync(CreateJobApplicationDto dto)
        {
            //Check that the specified job exists before creating the application
            var job = await _jobRepository.GetByIdWithCompanyAsync(dto.JobId);

            if (job == null)
            {
                throw new NotFoundException(nameof(JobApplication), dto.JobId);
            }


            // Ensure the job status is Accepted
            if (job.ApprovalStatus != JobApprovalStatus.Accepted)
            {
                throw new BadRequestException(
                    "You cannot apply for this job because it has not been accepted by the administrator.");
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
            jobApplication.DateApplied = DateTime.UtcNow;
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

            if (user != null)
            {
                await _emailSender.SendEmail(new EmailMessageData
                {
                    To = dto.Email,
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

            if (user != null)
            {
                await _emailSender.SendEmail(new EmailMessageData
                {
                    To = jobApplication.Email,
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
        //Only people associated with the Company of the posted job have the authority to perform this task
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


            //Ensure the company user is associated with the company that posted the job  
            string exceptionMessage = "Only users associated with the company who posted the job can delete the submitted applications.";
            await CheckUserAuthorization(job.CompanyId, exceptionMessage);


            await _jobApplicationRepository.DeleteAsync(jobApplication);
        }



        // Retrieves entities that match the specified search criteria
        //Students can only search their own applications
        //Company users can search applications for the jobs posted by their own company
        public async Task<List<JobApplicationDto>> SearchAsync(SearchDto dto)
        {
            IReadOnlyList<JobApplication> jobApplications;

            var userId = _currentUserService.UserId;
            bool isStudent = await GetUserRole("Student");
            bool isCompany = await GetUserRole("Company");

            var searchTerm = dto.SearchTerm?.Trim();

            if (string.IsNullOrEmpty(searchTerm))
            {
                // Return all entities without filtering
                return await GetPagedAsync(new PaginationDto());
            }
      

            if (isStudent)
            {
                jobApplications = await _jobApplicationRepository.SearchForUserAsync(searchTerm, userId);
            }

            else if (isCompany)
            {
                var companyId = await GetUserCompanyId();

                jobApplications = await _jobApplicationRepository.SearchForCompanyAsync(searchTerm, companyId);
            }


            else
            {
                throw new ForbiddenException("You are not authorized to view job applications.");
            }


            return _mapper.Map<List<JobApplicationDto>>(jobApplications);
        }




        // Retrieves entities sorted according to the specified sort option
        //Students can only sort their own applications
        //Company users can sort applications for the jobs posted by their own company
        public async Task<List<JobApplicationDto>> SortAsync(SortDto dto)
        {
            var userId = _currentUserService.UserId;
            int companyId = 0;
            bool isStudent = await GetUserRole("Student");
            bool isCompany = await GetUserRole("Company");

            //Validate the dto
            var validator = new SortValidator();
            var validationResult = await validator.ValidateAsync(dto);

            if (validationResult.Errors.Any())
            {
                throw new BadRequestException("Invalid sort option. ", validationResult);
            }


            if (isCompany)
            {
                companyId = await GetUserCompanyId();
            }


            IReadOnlyList<JobApplication> jobApplications;

            switch (dto.SortBy)
            {
                case SortOption.Ascending:
                    jobApplications = await _jobApplicationRepository.SortJobApplicationsAsync(
                        false,
                        x => x.FirstName,
                        isStudent ? userId : null,
                        isCompany ? companyId : null);

                    break;

                case SortOption.Descending:
                    jobApplications = await _jobApplicationRepository.SortJobApplicationsAsync(
                        true,
                        x => x.FirstName,
                        isStudent ? userId : null,
                        isCompany ? companyId : null);
                   
                    break;

                case SortOption.Latest:
                    jobApplications = await _jobApplicationRepository.SortJobApplicationsAsync(
                        true,
                        x => x.DateApplied,
                        isStudent ? userId : null,
                        isCompany ? companyId : null);
                    
                    break;

                case SortOption.Oldest:
                    jobApplications = await _jobApplicationRepository.SortJobApplicationsAsync(
                        false,
                        x => x.DateApplied,
                        isStudent ? userId : null,
                        isCompany ? companyId : null);
                    
                    break;

                default:
                    throw new ArgumentOutOfRangeException(
                    nameof(dto.SortBy),
                    dto.SortBy,
                    "Invalid sort option");
            }
            

            return _mapper.Map<List<JobApplicationDto>>(jobApplications);
        }    

    }
}
