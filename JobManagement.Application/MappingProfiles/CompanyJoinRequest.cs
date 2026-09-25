using AutoMapper;
using JobManagement.Application.DTOs.CompanyJoinRequest;

namespace JobManagement.Application.MappingProfiles;

public class CompanyJoinRequest : Profile
{
    public CompanyJoinRequest()
    {
        CreateMap<CompanyJoinRequestDto, CompanyJoinRequest>().ReverseMap();
        CreateMap<CreateCompanyJoinRequestDto, CompanyJoinRequest>().ReverseMap();
        CreateMap<ChangeCompanyJoinRequestStatusDto, CompanyJoinRequest>().ReverseMap();
    }
}
