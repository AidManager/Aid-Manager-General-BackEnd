using AidManager.API.Collaborate.Domain.Model.Entities;
using AidManager.API.Collaborate.Domain.Model.Queries;
using AidManager.API.Collaborate.Domain.Repositories;
using AidManager.API.Collaborate.Domain.Services;
using AidManager.API.Collaborate.Interfaces.REST.Resources;

namespace AidManager.API.Collaborate.Application.Internal.QueryServices;

public class EventQueryService(IEventRepository eventRepository) : IEventQueryService
{
    public Task<IEnumerable<Event>?> handle(GetAllEventsQuery query)
    {
        try
        {
            return eventRepository.ListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine("Error to obtain the events: " + e.Message);
            return null;
        }
    }

    public Task<IEnumerable<Event>?> handle(GetEventsByProjectId query)
    {
        try
        {
            return eventRepository.GetEventsByProjectId(query.ProjectId);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error to obtain events by project id: " + e.Message);
            return null;
        }
    }
}