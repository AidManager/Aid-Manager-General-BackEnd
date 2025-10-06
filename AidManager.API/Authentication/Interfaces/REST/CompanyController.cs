using System.Net.Mime;
using AidManager.API.Authentication.Domain.Model.Queries;
using AidManager.API.Authentication.Domain.Services;
using AidManager.API.Authentication.Interfaces.REST.Resources;
using AidManager.API.Authentication.Interfaces.REST.Transform;
using AidManager.API.ManageCosts.Interfaces.REST.Resources;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AidManager.API.Authentication.Interfaces;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class CompanyController(ICompanyCommandService companyCommandService, ICompanyQueryService companyQueryService) : ControllerBase
{
    
    [HttpGet("{companyId}")]
    [SwaggerOperation(
        Summary = "Get Company by Id",
        Description = "Get the company by Id",
        OperationId = "GetCompanyID"
    )]
    [SwaggerResponse(201, "Company Found", typeof(GetCompanyResource))]
    public async Task<IActionResult> GetCompanyById(int companyId)
    {
        var query = new GetCompanyByIdQuery(companyId);
        var company = await companyQueryService.Handle(query);
        if (company == null) return Ok(new { status_code = 404, message = "Company not found" });
        return Ok(new { status_code = 200, message = "Company found", company = CreateCompanyResourceFromEntityAssembler.ToResourceFromEntity(company) });
    }
    
    [HttpPut("{companyId}")]
    [SwaggerOperation(
        Summary = "Update Company",
        Description = "Update certain things for the company by Id",
        OperationId = "UpdateCompany"
    )]
    [SwaggerResponse(201, "Company Updated", typeof(GetCompanyResource))]
    public async Task<IActionResult> UpdateCompany(int companyId,[FromBody] UpdateCompanyResource resource)
    {
        var command = UpdateCompanyCommandFromResourceAssembler.ToCommandFromResource(resource, companyId);
        var result = await companyCommandService.Handle(command);
        if (!result) return Ok(new { status_code = 404, message = "Company not found" });
        return Ok(new { status_code = 200, message = "Company updated" });
    }
    
}