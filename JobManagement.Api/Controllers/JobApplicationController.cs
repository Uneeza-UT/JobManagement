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



        [HttpGet]
        public async Task<ActionResult<List<JobApplicationDto>>> GetAll([FromQuery] PaginationDto dto)
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


        [HttpGet("my-applications")]
        [Authorize(Roles = "Student")]
        [ProducesResponseType(403)]
        public async Task<ActionResult<List<JobApplicationDto>>> Get([FromQuery] PaginationDto dto)
        {
            var jobApplications = await _jobApplicationService.GetByUserAsync(dto);

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



        [HttpGet("job/{jobId}")]
        [Authorize(Roles = "Company")]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<JobApplicationDto>>> Get(int jobId, [FromQuery] PaginationDto dto)
        {
            var jobApplications = await _jobApplicationService.GetByJobAsync(jobId, dto);

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




        [HttpGet("{id}")]
        [ProducesResponseType(404)]
        public async Task<ActionResult<JobApplicationDto>> GetById(int id)
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
        public async Task<ActionResult> Post([FromBody] CreateJobApplicationDto dto)
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





        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<JobApplicationDto>>> Search(SearchDto dto)
        {
            var jobApplications = await _jobApplicationService.SearchAsync(dto);
            return jobApplications;
        }




        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<JobApplicationDto>>> Sort(SortDto dto)
        {
            var jobApplications = await _jobApplicationService.SortAsync(dto);
            return jobApplications;
        }
    }
}
