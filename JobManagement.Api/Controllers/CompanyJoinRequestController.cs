using JobManagement.Application.Contracts.Services;
using JobManagement.Application.DTOs.Common;
using JobManagement.Application.DTOs.CompanyJoinRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JobManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyJoinRequestController : ControllerBase
    {
        private readonly ICompanyJoinRequestService _companyJoinRequestService;

        public CompanyJoinRequestController(ICompanyJoinRequestService companyJoinRequestService)
        {
            _companyJoinRequestService = companyJoinRequestService;
        }



        [Authorize(Roles = "Company")]
        [HttpGet]
        public async Task<ActionResult<List<CompanyJoinRequestDto>>> Get([FromQuery] PaginationDto dto)
        {
            var joinRequests = await _companyJoinRequestService.GetPagedAsync(dto);

            if (joinRequests == null || joinRequests.Count == 0)
            {
                return Ok(new
                {
                    message = "No company join requests found for the requested page.",
                    data = new List<CompanyJoinRequestDto>()
                });
            }

            return Ok(joinRequests);
        }




        [Authorize(Roles = "Company")]
        [HttpGet("{id}")]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CompanyJoinRequestDto>> Get(int id)
        {
            var joinRequest= await _companyJoinRequestService.GetByIdAsync(id);
            return Ok(joinRequest);
        }





        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        public async Task<ActionResult> Post([FromBody] CreateCompanyJoinRequestDto dto)
        {
            var response = await _companyJoinRequestService.CreateAsync(dto);
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
        public async Task<ActionResult> Put([FromBody] ChangeCompanyJoinRequestStatusDto dto)
        {
            await _companyJoinRequestService.AcceptJoinRequestAsync(dto);
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
            await _companyJoinRequestService.DeleteAsync(id);
            return NoContent();
        }





        [HttpGet("search")]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<CompanyJoinRequestDto>>> Search([FromQuery] SearchDto dto)
        {
            var joinRequests = await _companyJoinRequestService.SearchAsync(dto);
            return joinRequests;
        }



        [HttpGet("sort")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<CompanyJoinRequestDto>>> Sort([FromQuery] SortDto dto)
        {
            var joinRequests = await _companyJoinRequestService.SortAsync(dto);
            return joinRequests;
        }
    }
}
