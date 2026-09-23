using AutoMapper;
using JobManagement.Application.DTOs.Job;
using JobManagement.Domain;

namespace JobManagement.Application.MappingProfiles;

public class JobProfile : Profile
{
    public JobProfile()
    {
        CreateMap<JobDto, Job>().ReverseMap();
        CreateMap<CreateJobDto, Job>().ReverseMap();
        CreateMap<UpdateJobDto, Job>().ReverseMap();
        CreateMap<ChangeJobApprovalStatusDto, Job>().ReverseMap();
    }
}
