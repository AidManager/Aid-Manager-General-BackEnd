using AidManager.API.Collaborate.Domain.Model.Entities;
using AidManager.API.Collaborate.Domain.Model.Queries;
using AidManager.API.Collaborate.Interfaces.REST.Resources;

namespace AidManager.API.Collaborate.Domain.Services;

public interface IEventQueryService
{
    Task<IEnumerable<Event>?> handle(GetAllEventsQuery query);
    Task<IEnumerable<Event>?> handle(GetEventsByProjectId query);
}