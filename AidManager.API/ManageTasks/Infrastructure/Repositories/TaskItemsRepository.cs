using System.Collections;
using AidManager.API.ManageTasks.Domain.Model.Aggregates;
using AidManager.API.ManageTasks.Domain.Repositories;
using AidManager.API.Shared.Infraestructure.Persistence.EFC.Configuration;
using AidManager.API.Shared.Infraestructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AidManager.API.ManageTasks.Infrastructure.Repositories;

public class TaskItemsRepository(AppDBContext context) : BaseRepository<TaskItem>(context), ITaskRepository
{

    public async Task<List<TaskItem>> GetTasksByProjectId(int projectId)
    {
        return await Context.Set<TaskItem>().Where(f => f.ProjectId == projectId).ToListAsync();
    }

    public async Task<TaskItem?> GetTaskById(int id)
    {
        return await Context.Set<TaskItem>().FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<List<TaskItem>> GetTasksByUserId(int userId)
    {
        return await Context.Set<TaskItem>().Where(f => f.AssigneeId == userId).ToListAsync();
    }
    
    public async Task<List<TaskItem>> GetTasksByUserIdAndProjectId(int userId, int projectId)
    {
        return await Context.Set<TaskItem>()
            .Where(f => f.AssigneeId == userId && f.ProjectId == projectId)
            .ToListAsync();
    }
    
}
