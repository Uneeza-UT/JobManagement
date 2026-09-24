using JobManagement.Application.Contracts.Services;
using JobManagement.Application.DTOs.Common;
using JobManagement.Application.DTOs.Job;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JobManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JobsController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobsController(IJobService jobService)
        {
            _jobService = jobService;
        }


        
        [HttpGet]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<JobDto>>> Get([FromQuery] PaginationDto dto)
        {
            var jobs = await _jobService.GetPagedAsync(dto);

            if (jobs == null || jobs.Count == 0)
            {
                return Ok(new
                {
                    message = "No jobs found for the requested page.",
                    data = new List<JobDto>()
                });
            }

            return Ok(jobs);
        }

        


        [HttpGet("{id}")]
        [ProducesResponseType(404)]
        public async Task<ActionResult<JobDto>> Get(int id)
        {
            var job = await _jobService.GetByIdAsync(id);
            return Ok(job);
        }



        [Authorize(Roles = "Company")]
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        public async Task<ActionResult> Post([FromBody]CreateJobDto dto)
        {
            var response = await _jobService.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = response });
        }




        [Authorize(Roles = "Company")]
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Put([FromBody] UpdateJobDto dto)
        {
            await _jobService.UpdateAsync(dto);
            return NoContent();
        }




        [Authorize(Roles = "Administrator")]
        [HttpPut("approval-status")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Put([FromBody] ChangeJobApprovalStatusDto dto)
        {
            await _jobService.ChangeApprovalStatus(dto);
            return NoContent();
        }




        [Authorize(Roles = "Administrator, Company")]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Delete(int id)
        {
            await _jobService.DeleteAsync(id);
            return NoContent();
        }




        [HttpGet("search")]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<JobDto>>> Search(SearchDto dto)
        {
            var jobs = await _jobService.SearchAsync(dto);
            return jobs;
        }



        [HttpGet("sort")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<JobDto>>> Sort(SortDto dto)
        {
            var jobs = await _jobService.SortAsync(dto);
            return jobs;
        }
    }
}
