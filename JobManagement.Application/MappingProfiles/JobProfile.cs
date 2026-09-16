using AutoMapper;
using JobManagement.Application.DTOs.Job;
using JobManagement.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobManagement.Application.MappingProfiles
{
    public class JobProfile : Profile
    {
        public JobProfile()
        {
            CreateMap<JobDto, Job>().ReverseMap();
        }
    }
}
