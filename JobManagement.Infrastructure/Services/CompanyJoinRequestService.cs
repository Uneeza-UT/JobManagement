using AutoMapper;
using JobManagement.Application.Contracts.Email;
using JobManagement.Application.Contracts.Persistence;
using JobManagement.Application.Contracts.Services;
using JobManagement.Application.DTOs.Common;
using JobManagement.Application.DTOs.CompanyJoinRequest;
using JobManagement.Application.Enums;
using JobManagement.Application.Exceptions;
using JobManagement.Application.Models.Email;
using JobManagement.Application.Validations.CompanyJoinRequest;
using JobManagement.Application.Validations.Sort;
using JobManagement.Domain;
using JobManagement.Domain.Enums;
using JobManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace JobManagement.Infrastructure.Services
{
    public class CompanyJoinRequestService : GenericService, ICompanyJoinRequestService
    {
        private readonly IMapper _mapper;
        private readonly ICompanyJoinRequestRepository _companyJoinRequestRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmailSender _emailSender;

        public CompanyJoinRequestService(ICompanyJoinRequestRepository companyJoinRequestRepository,
            IMapper mapper, IEmailSender emailSender,
            ICompanyRepository companyRepository,
            UserManager<ApplicationUser> userManager, 
            ICurrentUserService currentUserService) : base(userManager, currentUserService)
        {
            this._companyJoinRequestRepository = companyJoinRequestRepository;
            this._companyRepository = companyRepository;
            this._mapper = mapper;
            this._emailSender = emailSender;
        }


        //Retrieve the paginated list of all company join requests based on the user role
        //This list is only visible to the company owners and the users who applied for the request
        public async Task<List<CompanyJoinRequestDto>> GetPagedAsync(PaginationDto dto)
        {
            IReadOnlyList<CompanyJoinRequest> companyJoinRequests = new List<CompanyJoinRequest>();

            bool isStudent = await GetUserRole("Student");
            bool isCompany = await GetUserRole("Company");
            var userId = _currentUserService.UserId;


            if (isStudent)
            {
                companyJoinRequests = await _companyJoinRequestRepository
                    .GetByUserIdAsync(userId, dto.PageNumber, dto.PageSize);
            }

            else if (isCompany)
            {
                //Check if the user is the owner of a company
                var company = await _companyRepository.GetByUserIdAsync(userId);


                if (company == null)
                {
                    throw new NotFoundException(nameof(Company), "You do not own a company");
                }

                companyJoinRequests = await _companyJoinRequestRepository
                    .GetByCompanyIdAsync(company.Id, dto.PageNumber, dto.PageSize);
            }

            else
            {
                throw new ForbiddenException("You are not authorized to view company join requests.");
            }
            

            var data = _mapper.Map<List<CompanyJoinRequestDto>>(companyJoinRequests);
            return data;
        }



        //Retrieve a single company join requests
        //Ensures the user can only get his own requests
        //Ensures only the company owner can view the request sent to a company
        public async Task<CompanyJoinRequestDto> GetByIdAsync(int id)
        {
            bool isStudent = await GetUserRole("Student");
            bool isCompany = await GetUserRole("Company");
            var userId = _currentUserService.UserId;


            var joinRequest = await _companyJoinRequestRepository.GetByIdAsync(id);

            if (joinRequest == null)
            {
                throw new NotFoundException(nameof(CompanyJoinRequest), id);
            }


            if (isStudent)
            {
                if (joinRequest.UserId != userId)
                {
                    throw new ForbiddenException("You can only view your own join requests.");
                }
            }

            else if (isCompany)
            {
                //Check if the user is the owner of a company
                var company = await _companyRepository.GetByUserIdAsync(userId);


                if (company == null)
                {
                    throw new ForbiddenException("You do not own a company.");
                }

                if (joinRequest.CompanyId != company.Id)
                {
                    throw new ForbiddenException("You can only view your own company's join requests.");
                }
            }

            else
            {
                throw new ForbiddenException("You are not authorized to view company join requests.");
            }


            var data = _mapper.Map<CompanyJoinRequestDto>(joinRequest);
            return data;
        }

       


        public async Task<int> CreateAsync(CreateCompanyJoinRequestDto dto)
        {
            //Check if the comany exists
            var company = await _companyRepository.GetByIdAsync(dto.CompanyId);

            if (company == null)
            {
                throw new NotFoundException(nameof(Company), dto.CompanyId);
            }


            //Check if the user is already part of a company
            var userId = _currentUserService.UserId;

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new NotFoundException(nameof(ApplicationUser), userId);
            }


            if (user.CompanyId.HasValue)
            {
                throw new ConflictException("You are already part of a company.");
            }


            //Check for duplicate request to the same company
            bool existsJoinRequest = await _companyJoinRequestRepository.ExistsPendingRequest(dto.CompanyId, userId);

            if (existsJoinRequest)
            {
                throw new ConflictException("You already have a pending request to join this company.");
            }



            //Validate the dto
            var validator = new CreateCompanyJoinRequestValidator();
            var validationResult = await validator.ValidateAsync(dto);

            if (validationResult.Errors.Any())
            {
                throw new BadRequestException("Invalid join request. ", validationResult);
            }



            var joinRequest = new CompanyJoinRequest
            {
                UserId = userId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                CompanyId = dto.CompanyId,
                CreatedAt = DateTime.UtcNow
            };

            await _companyJoinRequestRepository.CreateAsync(joinRequest);



            //Notify the company owner about join request through email       
            
            var companyOwner = await _userManager.FindByIdAsync(company.OwnerUserId);


            if (companyOwner != null && !string.IsNullOrEmpty(companyOwner.Email))
            {
                await _emailSender.SendEmail(new EmailMessageData
                {
                    To = companyOwner.Email,
                    Subject = "New Company Join Request",
                    Body = $"Hello {companyOwner.FirstName},\n\n" +
                           $"A user has requested to join your company, {company.Name}.\n\n" +
                           $"Applicant: {user.FirstName} {user.LastName}.\n" +
                           $"Email: {user.Email}.\n" +
                           "Please log in to the Job Management System to review the join request and accept or reject it.\n\n" +
                           "Thank you,\n" +
                           "Job Management System"
                });
            }         

            return joinRequest.Id;

        }




        // Accepts or rejects a company join request and notifies the requesting user
        //Only the company owner can perform this task
        public async Task AcceptJoinRequestAsync(ChangeCompanyJoinRequestStatusDto dto)
        {      
            //Validate the dto
            var validator = new ChangeCompanyJoinRequestStatusValidator();
            var validationResult = await validator.ValidateAsync(dto);

            if (validationResult.Errors.Any())
            {
                throw new BadRequestException("Invalid request. ", validationResult);
            }


            var joinRequest = await _companyJoinRequestRepository.GetByIdAsync(dto.Id);

            if (joinRequest == null)
            {
                throw new NotFoundException(nameof(CompanyJoinRequest), dto.Id);
            }


            //Check if the user already belongs to a company
            var user = await _userManager.FindByIdAsync(joinRequest.UserId);

            if (user == null)
            {
                throw new NotFoundException(nameof(ApplicationUser), joinRequest.UserId);
            }

            if (dto.Status == JoinRequestStatus.Accepted && user.CompanyId != null)
            {
                throw new ConflictException("This user already belongs to another company.");
            }


            //Ensures only the owner can change the status of a join request

            var company = await _companyRepository.GetByUserIdAsync(_currentUserService.UserId);


            if (company == null)
            {
                throw new ForbiddenException("You do not own a company.");
            }


            if (joinRequest.CompanyId != company.Id)
            {
                throw new ForbiddenException("Only the company owner can change the status of join request.");
            }


            _mapper.Map(dto, joinRequest);
            await _companyJoinRequestRepository.UpdateAsync(joinRequest);
        
           
            


            //Update compayId of the user who requested to join the company

            if (dto.Status == JoinRequestStatus.Accepted)
            {
                user.CompanyId = company.Id;
                await _userManager.UpdateAsync(user);
            }


            //Notify the user about the status of their join request through email
            if (!string.IsNullOrEmpty(user.Email))
            {
                await _emailSender.SendEmail(new EmailMessageData
                {
                    To = user.Email,
                    Subject = "Job Posting Status Updated",
                    Body = $"Hello {user.FirstName},\n\n" +
                           $"Your request to join {company.Name} has been {joinRequest.Status.ToString().ToLower()}.\n\n" +
                           "You can log in to the Job Management System to view your request status.\n\n" +
                           "Thank you,\n" +
                           "Job Management System"
                });
            }
        }




        //Deletes a company join request entity
        //Only the company owner can perform this task
        public async Task DeleteAsync(int id)
        {
            var joinRequest = await _companyJoinRequestRepository.GetByIdAsync(id);

            if (joinRequest == null)
            {
                throw new NotFoundException(nameof(CompanyJoinRequest), id);
            }


            //Ensures only the owner can delete a join request

            var company = await _companyRepository.GetByUserIdAsync(_currentUserService.UserId);


            if (company == null)
            {
                throw new ForbiddenException("You do not own a company.");
            }


            if (joinRequest.CompanyId != company.Id)
            {
                throw new ForbiddenException("Only the company owner can delete the join request.");
            }


            await _companyJoinRequestRepository.DeleteAsync(joinRequest);
            
        }



        //Retrieves entities that match the specified search criteria
        //Students can only search their own requests
        //Company users can search requests for their own company
        public async Task<List<CompanyJoinRequestDto>> SearchAsync(SearchDto dto)
        {
            IReadOnlyList<CompanyJoinRequest> joinRequests;

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
                joinRequests = await _companyJoinRequestRepository.SearchForUserAsync(searchTerm, userId);
            }

            else if (isCompany)
            {
                var companyId = await GetUserCompanyId();

                joinRequests = await _companyJoinRequestRepository.SearchForCompanyAsync(searchTerm, companyId);
            }


            else
            {
                throw new ForbiddenException("You are not authorized to view company's join requests.");
            }


            return _mapper.Map<List<CompanyJoinRequestDto>>(joinRequests);
        }




        //Retrieves entities sorted according to the specified sort option
        //Students can only sort their own requests
        //Company users can sort requests for their own company
        public async Task<List<CompanyJoinRequestDto>> SortAsync(SortDto dto)
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


            IReadOnlyList<CompanyJoinRequest> joinRequests;

            switch (dto.SortBy)
            {
                case SortOption.Ascending:
                    joinRequests = await _companyJoinRequestRepository.SortJoinRequestsAsync(
                        false,
                        x => x.FirstName,
                        isStudent ? userId : null,
                        isCompany ? companyId : null);

                    break;

                case SortOption.Descending:
                    joinRequests = await _companyJoinRequestRepository.SortJoinRequestsAsync(
                        true,
                        x => x.FirstName,
                        isStudent ? userId : null,
                        isCompany ? companyId : null);

                    break;

                case SortOption.Latest:
                    joinRequests = await _companyJoinRequestRepository.SortJoinRequestsAsync(
                        true,
                        x => x.CreatedAt,
                        isStudent ? userId : null,
                        isCompany ? companyId : null);

                    break;

                case SortOption.Oldest:
                    joinRequests = await _companyJoinRequestRepository.SortJoinRequestsAsync(
                        false,
                        x => x.CreatedAt,
                        isStudent ? userId : null,
                        isCompany ? companyId : null);

                    break;

                default:
                    throw new ArgumentOutOfRangeException(
                    nameof(dto.SortBy),
                    dto.SortBy,
                    "Invalid sort option");
            }

            return _mapper.Map<List<CompanyJoinRequestDto>>(joinRequests);
        }
    }
}
