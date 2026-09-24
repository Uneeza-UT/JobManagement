using AutoMapper;
using JobManagement.Application.Contracts.Email;
using JobManagement.Application.Contracts.Persistence;
using JobManagement.Application.Contracts.Services;
using JobManagement.Application.DTOs.Common;
using JobManagement.Application.DTOs.Company;
using JobManagement.Application.Enums;
using JobManagement.Application.Exceptions;
using JobManagement.Application.Models.Email;
using JobManagement.Application.Validations.Company;
using JobManagement.Application.Validations.Sort;
using JobManagement.Domain;
using JobManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace JobManagement.Infrastructure.Services
{
    public class CompanyService : GenericService, ICompanyService
    {
        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmailSender _emailSender;

        public CompanyService(ICompanyRepository companyRepository, IMapper mapper,
            IEmailSender emailSender,
            UserManager<ApplicationUser> userManager,
            ICurrentUserService currentUserService) : base(userManager, currentUserService)
        {
            this._companyRepository = companyRepository;
            this._mapper = mapper;
            this._emailSender = emailSender;
        }


        //Retrieve the paginated list of all database entities
        public async Task<List<CompanyDto>> GetPagedAsync(PaginationDto dto)
        {
            var companies = await _companyRepository.GetPagedAsync(dto.PageNumber, dto.PageSize);
            var data = _mapper.Map<List<CompanyDto>>(companies);
            return data;
        }



        //Retrieve a single company entity using the Id
        public async Task<CompanyDto> GetByIdAsync(int id)
        {
            var company = await _companyRepository.GetByIdAsync(id);

            if (company == null)
            {
                throw new NotFoundException(nameof(Company), id);
            }

            var data = _mapper.Map<CompanyDto>(company);
            return data;
        }



        //Creates a new company entity
        //Only a user with "Company" role can create a company
        public async Task<int> CreateAsync(CreateCompanyDto dto)
        {
            //Validate the dto
            var validator = new CreateCompanyValidator();
            var validationResult = await validator.ValidateAsync(dto);

            if (validationResult.Errors.Any())
            {
                throw new BadRequestException("Invalid company entity. ", validationResult);
            }


            //Ensure that no other company is registered with the same email address
            bool isEmailUnique = await _companyRepository.IsCompanyEmailUniqueAsync(dto.Email);

            if (!isEmailUnique)
            {
                throw new ConflictException("A company with this email address already exists.");
            }


            var company = _mapper.Map<Company>(dto);
            company.CreatedAt = DateTime.Now;

            var userId = _currentUserService.UserId;

            company.OwnerUserId = _currentUserService.UserId;
            await _companyRepository.CreateAsync(company);


            // Link the user's Identity account to the company they registered           
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new NotFoundException(nameof(ApplicationUser), userId);
            }

            user.CompanyId = company.Id;
            await _userManager.UpdateAsync(user);

            return company.Id;
        }



        //Updates a company entity
        //Only a user with "Company" role can update their own company
        public async Task UpdateAsync(UpdateCompanyDto dto)
        {
            var company = await _companyRepository.GetByIdAsync(dto.Id);

            if (company == null)
            {
                throw new NotFoundException(nameof(Company), dto.Id);
            }


            // Ensure a user can only update their own company

            string exceptionMessage = "You can only update your own company.";
            await CheckUserAuthorization(dto.Id, exceptionMessage);



            //Validate the dto
            var validator = new UpdateCompanyValidator();
            var validationResult = await validator.ValidateAsync(dto);

            if (validationResult.Errors.Any())
            {
                throw new BadRequestException("Invalid company entity. ", validationResult);
            }



            // Ensure the email is not already associated with another company
            bool isEmailUnique = await _companyRepository.IsCompanyEmailUniqueAsync(dto.Email, company.Id);

            if (!isEmailUnique)
            {
                throw new ConflictException("A company with this email address already exists.");
            }


            _mapper.Map(dto, company);
            company.UpdatedAt = DateTime.Now;
            await _companyRepository.UpdateAsync(company);
        }



        //Deletes a company entity
        //Only a user with "Company" role can delete their own company
        public async Task DeleteAsync(int id)
        {
            var company = await _companyRepository.GetByIdAsync(id);

            if (company == null)
            {
                throw new NotFoundException(nameof(Company), id);
            }


            //Ensures the user can only delete their own company

            string exceptionMessage = "You can only delete your own company.";
            await CheckUserAuthorization(id, exceptionMessage);

            await _companyRepository.DeleteAsync(company);


            //Notify the user about deletion of company profile through email
            var user = await _userManager.FindByIdAsync(company.OwnerUserId);

            if (user != null && !string.IsNullOrEmpty(user.Email))
            {
                await _emailSender.SendEmail(new EmailMessageData
                {
                    To = user.Email,
                    Subject = "Job Posting Status Updated",
                    Body = $"Hello {user.FirstName},\n\n" +
                           $"Your company profile, {company.Name}, has been successfully deleted from the Job Management System.\n" +
                           "Your associated job postings and applications have not been deleted.\n\n" +
                           "Thank you,\n" +
                           "Job Management System"
                });
            }
        }



        // Associates the specified user with the company of the currently logged-in user
        public async Task AssignCompanyToUserAsync(AssignCompanyToUserDto dto)
        {
            var companyId = await GetUserCompanyId();

            var user = await _userManager.FindByIdAsync(dto.UserId);

            if (user == null)
            {
                throw new NotFoundException(nameof(ApplicationUser), dto.UserId);
            }


            //Checks if the user is associated with a company
            if (user.CompanyId != null)
            {
                throw new ForbiddenException("This user is already associated with a company.");
            }

            user.CompanyId = companyId;

            await _userManager.UpdateAsync(user);
        }


        // Retrieves entities that match the specified search criteria.
        public async Task<List<CompanyDto>> SearchAsync(SearchDto dto)
        {
            var searchTerm = dto.SearchTerm?.Trim();

            if (string.IsNullOrEmpty(searchTerm))
            {
                // Return all entities without filtering
                return await GetPagedAsync(new PaginationDto());
            }

            var companies = await _companyRepository.SearchAsync(searchTerm);

            return _mapper.Map<List<CompanyDto>>(companies);
        }




        // Retrieves entities sorted according to the specified sort option.
        public async Task<List<CompanyDto>> SortAsync(SortDto dto)
        {
            //Validate the dto
            var validator = new SortValidator();
            var validationResult = await validator.ValidateAsync(dto);

            if (validationResult.Errors.Any())
            {
                throw new BadRequestException("Invalid search term. ", validationResult);
            }


            IReadOnlyList<Company> data;

            switch (dto.SortBy)
            {
                case SortOption.Ascending:
                    data = await _companyRepository.SortAsync(false, x => x.Name);
                    break;

                case SortOption.Descending:
                    data = await _companyRepository.SortAsync(true, x => x.Name);
                    break;

                case SortOption.Latest:
                    data = await _companyRepository.SortAsync(true, x => x.CreatedAt);
                    break;

                case SortOption.Oldest:
                    data = await _companyRepository.SortAsync(false, x => x.CreatedAt);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(
                    nameof(dto.SortBy),
                    dto.SortBy,
                    "Invalid sort option.");
            }

            return _mapper.Map<List<CompanyDto>>(data);
        }

    }
}
