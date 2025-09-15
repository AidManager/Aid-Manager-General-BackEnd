using AidManager.API.Collaborate.Domain.Model.Entities;
using AidManager.API.Collaborate.Interfaces.REST.Resources;
using AidManager.API.Shared.Domain.Repositories;

namespace AidManager.API.Collaborate.Domain.Repositories;

public interface IEventRepository : IBaseRepository<Event>
{
    Task<IEnumerable<Event>?> GetEventsByProjectId(int projectId);
    Task<Event?> GetEventById(int eventId);
}