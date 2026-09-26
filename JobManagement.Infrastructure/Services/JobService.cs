using AutoMapper;
using JobManagement.Application.Contracts.Email;
using JobManagement.Application.Contracts.Persistence;
using JobManagement.Application.Contracts.Services;
using JobManagement.Application.DTOs.Common;
using JobManagement.Application.DTOs.Job;
using JobManagement.Application.Enums;
using JobManagement.Application.Exceptions;
using JobManagement.Application.Models.Email;
using JobManagement.Application.Validations.Job;
using JobManagement.Application.Validations.Sort;
using JobManagement.Domain;
using JobManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;


namespace JobManagement.Infrastructure.Services
{
    public class JobService : GenericService, IJobService
    {
        private readonly IMapper _mapper;
        private readonly IJobRepository _jobRepository;
        private readonly IEmailSender _emailSender;

        public JobService(IJobRepository jobRepository, IMapper mapper,
            IEmailSender emailSender,
            UserManager<ApplicationUser> userManager, 
            ICurrentUserService currentUserService) : base(userManager, currentUserService)
        {
            this._jobRepository = jobRepository;
            this._mapper = mapper;
            this._emailSender = emailSender;
        }


        // Retrieves all jobs and applies pagination
        public async Task<List<JobDto>> GetPagedAsync(PaginationDto dto)
        {

            var jobs = await _jobRepository.GetPagedAsync(dto.PageNumber, dto.PageSize);          
            var data = _mapper.Map<List<JobDto>>(jobs);
            return data;
        }



        // Retrieves a single job entity
        public async Task<JobDto> GetByIdAsync(int id)
        {
            var job = await _jobRepository.GetByIdAsync(id);

            if (job == null)
            {
                throw new NotFoundException(nameof(Job), id);
            }


            var data = _mapper.Map<JobDto>(job);
            return data;
        }



        //Creates a new job entity
        //Only a user with "Company" role can create a job
        public async Task<int> CreateAsync(CreateJobDto dto)
        {
            var companyId = await GetUserCompanyId();


            //Validate the dto
            var validator = new CreateJobValidator();
            var validationResult = await validator.ValidateAsync(dto);

            if (validationResult.Errors.Any())
            {
                throw new BadRequestException("Invalid job entity. ", validationResult);
            }


            // Ensure the company does not have more than 3 accepted jobs with the same title 
            var jobsCount = await _jobRepository.CountActiveJobsWithSameTitleAsync(dto.Title, companyId);

            if (jobsCount >= 3)
            {
                throw new ConflictException("You cannot have more than 3 active job postings with the same title.");
            }


            var job = _mapper.Map<Job>(dto);
            job.CompanyId = companyId;
            job.CreatedAt = DateTime.UtcNow;
            job.PostedByUserId = _currentUserService.UserId;

            await _jobRepository.CreateAsync(job);
            return job.Id;
        }



        //Updates a job entity
        //Only a user with "Company" role can update a job
        public async Task UpdateAsync(UpdateJobDto dto)
        {
            //Validate the dto
            var validator = new UpdateJobValidator();
            var validationResult = await validator.ValidateAsync(dto);

            if (validationResult.Errors.Any())
            {
                throw new BadRequestException("Invalid job entity. ", validationResult);
            }


            var job = await _jobRepository.GetByIdAsync(dto.Id);

            if (job == null)
            {
                throw new NotFoundException(nameof(Job), dto.Id);
            }


            // Ensure a user can only update a job posted by their own company

            string exceptionMessage = "You can only update a job posted by your own company.";
            await CheckUserAuthorization(job.CompanyId, exceptionMessage);



            // Ensure the company does not exceed 3 accepted jobs with the same title,
            // excluding the job currently being updated

            int companyId = await GetUserCompanyId();
            var jobsCount = await _jobRepository.CountActiveJobsWithSameTitleAsync(dto.Title, companyId, job.Id);

            if (jobsCount >= 3)
            {
                throw new ConflictException("You cannot have more than 3 active job postings with the same title.");
            }


            _mapper.Map(dto, job);
            job.UpdatedAt = DateTime.UtcNow;
            await _jobRepository.UpdateAsync(job);
        }



        //Change the status of job entity
        //Only Admins have the authority to perform this task
        public async Task ChangeApprovalStatus(ChangeJobApprovalStatusDto dto)
        {
            //Validate the dto
            var validator = new ChangeJobApprovalStatusValidator();
            var validationResult = await validator.ValidateAsync(dto);

            if (validationResult.Errors.Any())
            {
                throw new BadRequestException("Invalid job approval status. ", validationResult);
            }


            var job = await _jobRepository.GetByIdAsync(dto.Id);

            if (job == null)
            {
                throw new NotFoundException(nameof(Job), dto.Id);
            }


            // Ensure the application deadline has not already passed
            if (job.ApplicationDeadline <= DateTime.Now)
            {
                throw new BadRequestException(
                    "The job cannot be approved because its application deadline has passed.");
            }


            _mapper.Map(dto, job);
            await _jobRepository.UpdateAsync(job);


            //Notify the user about job status change through email
            var user = await _userManager.FindByIdAsync(job.PostedByUserId);

            if (user != null && !string.IsNullOrEmpty(user.Email))
            { 
                await _emailSender.SendEmail(new EmailMessageData
                {
                    To = user.Email,
                    Subject = "Job Posting Status Updated",
                    Body = $"Hello {user.FirstName},\n\n" +
                           $"The status of your job posting, {job.Title}, has been updated.\n" +
                           $"New status: {job.ApprovalStatus}.\n" +
                           "Please log in to the Job Management System to view the details of your job posting.\n\n" +
                           "Thank you,\n" +
                           "Job Management System"
                });
            }
        }  



        //Deletes the job entity
        public async Task DeleteAsync(int id)
        {
            var job = await _jobRepository.GetByIdAsync(id);

            if (job == null)
            {
                throw new NotFoundException(nameof(Job), id);
            }


            bool isAdmin = await GetUserRole("Administrator");
            bool isCompany = await GetUserRole("Company");


            if (isAdmin)
            {
                // Admin can delete any job
                await _jobRepository.DeleteAsync(job);


                //Notify the user about job deletion by admin through email
                var user = await _userManager.FindByIdAsync(job.PostedByUserId);

                if (user != null && !string.IsNullOrEmpty(user.Email))
                {
                    await _emailSender.SendEmail(new EmailMessageData
                    {
                        To = user.Email,
                        Subject = "Job Posting Deleted by Administrator",
                        Body = $"Hello {user.FirstName},\n\n" +
                               $"Your job posting, {job.Title}, has been deleted by an administrator from the Job Management System.\n" +
                               "The job posting is no longer available in the system.\n" +
                               "If you have any questions regarding this action, please contact the system administrator.\n\n" +
                               "Thank you,\n" +
                               "Job Management System"
                    });
                }
            }


            else if (isCompany)
            {
                // Ensure a user can only delete a job posted by their own company

                string exceptionMessage = "You can only delete a job posted by your own company.";
                await CheckUserAuthorization(job.CompanyId, exceptionMessage);

                await _jobRepository.DeleteAsync(job);
            }

            else
            {
                throw new ForbiddenException("You are not authorized to delete jobs.");
            }
        }



        // Retrieves entities that match the specified search criteria.
        public async Task<List<JobDto>> SearchAsync(SearchDto dto)
        {
            var searchTerm = dto.SearchTerm?.Trim();

            if (string.IsNullOrEmpty(searchTerm))
            {
                // Return all entities without filtering
                return await GetPagedAsync(new PaginationDto());
            }

            var jobs = await _jobRepository.SearchAsync(searchTerm);

            return _mapper.Map<List<JobDto>>(jobs);
        }
        



        // Retrieves entities sorted according to the specified sort option.
        public async Task<List<JobDto>> SortAsync(SortDto dto)
        {
            //Validate the dto
            var validator = new SortValidator();
            var validationResult = await validator.ValidateAsync(dto);

            if (validationResult.Errors.Any())
            {
                throw new BadRequestException("Invalid search term. ", validationResult);
            }


            IReadOnlyList<Job> data;

            switch (dto.SortBy)
            {
                case SortOption.Ascending:
                    data = await _jobRepository.SortAsync(false, x => x.Title);
                    break;

                case SortOption.Descending:
                    data = await _jobRepository.SortAsync(true, x => x.Title);
                    break;

                case SortOption.Latest:
                    data = await _jobRepository.SortAsync(true, x => x.CreatedAt);
                    break;

                case SortOption.Oldest:
                    data = await _jobRepository.SortAsync(false, x => x.CreatedAt);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(
                    nameof(dto.SortBy),
                    dto.SortBy,
                    "Invalid sort option.");
            }

            return _mapper.Map<List<JobDto>>(data);
        }

    }
}
