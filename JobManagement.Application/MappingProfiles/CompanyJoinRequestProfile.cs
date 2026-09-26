using AutoMapper;
using JobManagement.Application.DTOs.CompanyJoinRequest;
using JobManagement.Domain;

namespace JobManagement.Application.MappingProfiles;

public class CompanyJoinRequestProfile : Profile
{
    public CompanyJoinRequestProfile()
    {
        CreateMap<CompanyJoinRequestDto, CompanyJoinRequest>().ReverseMap();
        CreateMap<CreateCompanyJoinRequestDto, CompanyJoinRequest>().ReverseMap();
        CreateMap<ChangeCompanyJoinRequestStatusDto, CompanyJoinRequest>().ReverseMap();
    }
}
