using AutoMapper;
using JobManagement.Application.DTOs.Company;
using JobManagement.Domain;

namespace JobManagement.Application.MappingProfiles;


public class CompanyProfile : Profile
{
    public CompanyProfile()
    {
        CreateMap<CompanyDto, Company>().ReverseMap();
        CreateMap<CreateCompanyDto, Company>().ReverseMap();
        CreateMap<UpdateCompanyDto, Company>().ReverseMap();
    }
}
