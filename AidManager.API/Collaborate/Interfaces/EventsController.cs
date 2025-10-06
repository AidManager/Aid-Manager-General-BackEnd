using System.Net.Mime;
using AidManager.API.Collaborate.Domain.Model.Commands;
using AidManager.API.Collaborate.Domain.Model.Queries;
using AidManager.API.Collaborate.Domain.Services;
using AidManager.API.Collaborate.Interfaces.REST.Resources;
using AidManager.API.Collaborate.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace AidManager.API.Collaborate.Interfaces;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class EventsController(IEventCommandService eventCommandService, IEventQueryService eventQueryService) : ControllerBase
{
    
    [HttpPost]
    public async Task<ActionResult> CreateNewEvent([FromBody] CreateEventResource resource)
    {
        var createEventCommand = CreateEventCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await eventCommandService.handle(createEventCommand);
        if(!result) return Ok(new {status_code = "500", message = "Event not created"});
        return Ok(new {status_code = "202", message = "Event created"});
    }
    
    [HttpGet]
    public async Task<IEnumerable<GetEventResource>?> GetAllEvents()
    {
        var events = await eventQueryService.handle(new GetAllEventsQuery());
        if (events == null) return [];
        var result = events.Select(EventResourceFromEntityAssembler.ToResourceFromEntity);
        return result;
    }
    
    [HttpGet("{projectId}")]
    public async Task<IEnumerable<GetEventResource>?> GetEventsByProjectId(int projectId)
    {
        var events = await eventQueryService.handle(new GetEventsByProjectId(projectId));
        if (events == null) return [];
        var result = events.Select(EventResourceFromEntityAssembler.ToResourceFromEntity);
        return result;
    }
    
    [HttpPut("{eventId}")]
    public async Task<ActionResult> UpdateEvent(int eventId, [FromBody] UpdateEventResource resource)
    {
        var updateEventCommand = UpdateEventCommandFromResourceAssembler.ToCommandFromResource(eventId, resource);
        var result = await eventCommandService.handle(updateEventCommand);
        if(!result) return Ok(new {status_code = "500", message = "Event not updated"});
        return Ok(new {status_code = "202", message = "Event updated"});
    }

    [HttpDelete("{eventId}")]
    public async Task<ActionResult> DeleteEvent(int eventId)
    {
        var result = await eventCommandService.handle(new DeleteEventCommand(eventId));
        if(!result) return Ok(new {status_code = "500", message = "Event not deleted"});
        return Ok(new {status_code = "202", message = "Event deleted"});
    }
}