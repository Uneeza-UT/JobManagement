using AutoMapper;
using JobManagement.Application.DTOs.JobApplication;
using JobManagement.Domain;

namespace JobManagement.Application.MappingProfiles;

public class JobApplicationProfile : Profile
{
    public JobApplicationProfile()
    {
        CreateMap<JobApplicationDto, JobApplication>().ReverseMap();
        CreateMap<CreateJobApplicationDto, JobApplication>().ReverseMap();
        CreateMap<ChangeJobApplicationStatusDto, JobApplication>().ReverseMap();
    }
}
