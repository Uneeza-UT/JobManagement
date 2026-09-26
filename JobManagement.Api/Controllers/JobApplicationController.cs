using JobManagement.Application.Contracts.Services;
using JobManagement.Application.DTOs.Common;
using JobManagement.Application.DTOs.JobApplication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JobManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JobApplicationController : ControllerBase
    {
        private readonly IJobApplicationService _jobApplicationService;

        public JobApplicationController(IJobApplicationService jobApplicationServic)
        {
            _jobApplicationService = jobApplicationServic;
        }



        [Authorize(Roles = "Student, Company")]
        [HttpGet]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<JobApplicationDto>>> Get([FromQuery] PaginationDto dto)
        {
            var jobApplications = await _jobApplicationService.GetPagedAsync(dto);

            if (jobApplications == null || jobApplications.Count == 0)
            {
                return Ok(new
                {
                    message = "No job applications found for the requested page.",
                    data = new List<JobApplicationDto>()
                });
            }

            return Ok(jobApplications);
        }




        [Authorize(Roles = "Student, Company")]
        [HttpGet("{id}")]      
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<JobApplicationDto>> Get(int id)
        {
            var jobApplication = await _jobApplicationService.GetByIdAsync(id);
            return Ok(jobApplication);
        }




        [Authorize(Roles = "Student")]
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        public async Task<ActionResult> Post([FromForm] CreateJobApplicationDto dto)
        {
            var response = await _jobApplicationService.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = response });
        }






        [Authorize(Roles = "Company")]
        [HttpPut("status")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Put([FromBody] ChangeJobApplicationStatusDto dto)
        {
            await _jobApplicationService.ChangeStatus(dto);
            return NoContent();
        }




        [Authorize(Roles = "Company")]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Delete(int id)
        {
            await _jobApplicationService.DeleteAsync(id);
            return NoContent();
        }




        [HttpGet("search")]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<JobApplicationDto>>> Search([FromQuery] SearchDto dto)
        {
            var jobApplications = await _jobApplicationService.SearchAsync(dto);
            return jobApplications;
        }



        [HttpGet("sort")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<JobApplicationDto>>> Sort([FromQuery] SortDto dto)
        {
            var jobApplications = await _jobApplicationService.SortAsync(dto);
            return jobApplications;
        }
    }
}
