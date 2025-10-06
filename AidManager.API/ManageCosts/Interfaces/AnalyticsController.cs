using System.Net.Mime;
using AidManager.API.ManageCosts.Domain.Model.Queries;
using AidManager.API.ManageCosts.Domain.Services;
using AidManager.API.ManageCosts.Interfaces.REST.Resources;
using AidManager.API.ManageCosts.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AidManager.API.ManageCosts.Interfaces.REST;

[ApiController]
[Route("api/v1/Projects/{projectId}/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class AnalyticsController(
    IAnalyticsCommandService analyticsCommandService,
    IAnalyticsQueryService analyticsQueryService
    ) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get Analytics by Project Id",
        Description = "gets Analytics by Project Id",
        OperationId = "GetAnalytics"
    )]
    [SwaggerResponse(200, "Analytics found", typeof(AnalyticsResource))]
    public async Task<IActionResult> GetAnalyticsByProjectId([FromRoute] int projectId)
    {
        var query = new GetAnalyticsByProjectId(projectId);
        var analytic = await analyticsQueryService.Handle(query);
        
        if (analytic == null)
        {
            return NotFound();
        }
        
        var analyticResource = AnalyticsResourceFromEntityAssembler.ToResourceFromEntity(analytic);
        return Ok(analyticResource);
    }
    
    [HttpPatch("lines")]
    [SwaggerOperation(
        Summary = "Update BarData",
        Description = "Update the BarData list",
        OperationId = "UpdateBarData"
    )]
    [SwaggerResponse(200, "Analytics updated", typeof(UpdateLinesChartBarResource))]
    public async Task<IActionResult> UpdateAnalyticPayments([FromRoute] int projectId, [FromBody] UpdateLinesChartBarResource resource)
    {
        var command = UpdateLinesChartBarCommandFromResourceAssembler.ToCommandFromResource(projectId, resource);
        var analytic = await analyticsCommandService.Handle(command);
        
        if (analytic == null)
        {
            return NotFound();
        }
        
        var analyticResource = AnalyticsResourceFromEntityAssembler.ToResourceFromEntity(analytic);
        return Ok(analyticResource);
    }
    
    [HttpPatch("bardata")]
    [SwaggerOperation(
        Summary = "Update LineBarData",
        Description = "Update the LineBarData list",
        OperationId = "UpdateLineBarData"
    )]
    [SwaggerResponse(200, "Analytics updated", typeof(UpdateBarDataResource))]
    public async Task<IActionResult> UpdateAnalyticLines([FromRoute] int projectId, [FromBody] UpdateBarDataResource resource)
    {
        var command = UpdateBarDataCommandFromResourceAssembler.ToCommandFromResource(projectId, resource);
        var analytic = await analyticsCommandService.Handle(command);
        
        if (analytic == null)
        {
            return NotFound();
        }
        
        var analyticResource = AnalyticsResourceFromEntityAssembler.ToResourceFromEntity(analytic);
        return Ok(analyticResource);
    }
    
    [HttpPatch("tasks")]
    [SwaggerResponse(200, "Analytics updated", typeof(UpdateAnalyticTasksResource))]
    public async Task<IActionResult> UpdateAnalyticTasks([FromRoute] int projectId,[FromBody] UpdateAnalyticTasksResource resource)
    {
        var command = UpdateAnalyticTasksCommandFromResourceAssembler.ToCommandFromResource(projectId, resource);
        var analytic = await analyticsCommandService.Handle(command);
        
        if (analytic == null)
        {
            return NotFound();
        }
        
        var analyticResource = AnalyticsResourceFromEntityAssembler.ToResourceFromEntity(analytic);
        return Ok(analyticResource);
    }
    
    [HttpPatch("progressbar")]
    [SwaggerResponse(200, "Analytics updated", typeof(UpdateAnalyticProgressbarResource))]
    public async Task<IActionResult> UpdateAnalyticProgressbar([FromRoute] int projectId,[FromBody] UpdateAnalyticProgressbarResource resource)
    {
        var command = UpdateAnalyticProgressbarCommandFromResourceAssembler.ToCommandFromResource(projectId, resource);
        var analytic = await analyticsCommandService.Handle(command);
        
        if (analytic == null)
        {
            return NotFound();
        }
        
        var analyticResource = AnalyticsResourceFromEntityAssembler.ToResourceFromEntity(analytic);
        return Ok(analyticResource);
    }
    
    [HttpPatch("status")]
    [SwaggerResponse(200, "Analytics updated", typeof(UpdateAnalyticStatusResource))]
    public async Task<IActionResult> UpdateAnalyticStatus([FromRoute] int projectId,[FromBody] UpdateAnalyticStatusResource resource)
    {
        var command = UpdateAnalyticStatusCommandFromResourceAssembler.ToCommandFromResource(projectId, resource);
        var analytic = await analyticsCommandService.Handle(command);
        
        if (analytic == null)
        {
            return NotFound();
        }
        
        var analyticResource = AnalyticsResourceFromEntityAssembler.ToResourceFromEntity(analytic);
        return Ok(analyticResource);
    }
    
    
    [HttpGet("/api/v1/Analytics-by-Company/{companyId}")]
    [SwaggerOperation(
        Summary = "Get Analytics by Company Id",
        Description = "gets Analytics by Company Id",
        OperationId = "GetAnalyticsByCompany"
    )]
    [SwaggerResponse(200, "Analytics found", typeof(AnalyticsResource))]
    public async Task<IActionResult> GetAnalyticsByCompanyId([FromRoute] int companyId)
    {
        var query = new GetAnalyticsByCompanyId(companyId);
        var analytic = await analyticsQueryService.Handle(query);
        
        if (analytic == null)
        {
            return BadRequest();
        }
        
        
        var analyticResources = analytic.Select(AnalyticsResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(analyticResources);
    }
    
}