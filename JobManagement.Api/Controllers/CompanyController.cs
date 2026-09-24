using JobManagement.Application.Contracts.Services;
using JobManagement.Application.DTOs.Common;
using JobManagement.Application.DTOs.Company;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JobManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }



        [HttpGet]
        public async Task<ActionResult<List<CompanyDto>>> Get([FromQuery] PaginationDto dto)
        {
            var companies = await _companyService.GetPagedAsync(dto);

            if (companies == null || companies.Count == 0)
            {
                return Ok(new
                {
                    message = "No companies found for the requested page.",
                    data = new List<CompanyDto>()
                });
            }

            return Ok(companies);
        }




        [HttpGet("{id}")]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CompanyDto>> Get(int id)
        {
            var company = await _companyService.GetByIdAsync(id);
            return Ok(company);
        }




        [Authorize(Roles = "Company")]
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        public async Task<ActionResult> Post([FromBody] CreateCompanyDto dto)
        {
            var response = await _companyService.CreateAsync(dto);
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
        public async Task<ActionResult> Put([FromBody] UpdateCompanyDto dto)
        {
            await _companyService.UpdateAsync(dto);
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
            await _companyService.DeleteAsync(id);
            return NoContent();
        }



        // Associates the specified user with the company of the currently logged-in user
        [Authorize(Roles = "Company")]
        [HttpPut("assign-company")]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Put([FromBody] AssignCompanyToUserDto dto)
        {
            await _companyService.AssignCompanyToUserAsync(dto);
            return NoContent();
        }



        [HttpGet("search")]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<CompanyDto>>> Search(SearchDto dto)
        {
            var companies = await _companyService.SearchAsync(dto);
            return companies;
        }



        [HttpGet("sort")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<CompanyDto>>> Sort(SortDto dto)
        {
            var companies = await _companyService.SortAsync(dto);
            return companies;
        }
    }
}
