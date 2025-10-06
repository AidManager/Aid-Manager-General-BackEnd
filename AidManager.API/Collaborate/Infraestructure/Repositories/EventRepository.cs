using AidManager.API.Collaborate.Domain.Model.Entities;
using AidManager.API.Collaborate.Domain.Repositories;
using AidManager.API.Collaborate.Interfaces.REST.Resources;
using AidManager.API.Shared.Infraestructure.Persistence.EFC.Configuration;
using AidManager.API.Shared.Infraestructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AidManager.API.Collaborate.Infraestructure.Repositories;

public class EventRepository(AppDBContext context) : BaseRepository<Event>(context), IEventRepository
{
    
    public async Task<IEnumerable<Event>?> GetEventsByProjectId(int projectId)
    {
        return await Context.Set<Event>().Where(e => e.ProjectId == projectId).ToListAsync();
    }

    public async Task<Event?> GetEventById(int eventId)
    {
        try
        {
            return await Context.Set<Event>().FindAsync(eventId);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error to obtain the event by id");
            return null;
        }
    }
}