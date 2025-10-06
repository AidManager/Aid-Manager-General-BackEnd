using AidManager.API.ManageTasks.Domain.Model.Aggregates;
using AidManager.API.ManageTasks.Domain.Repositories;
using AidManager.API.Shared.Domain.Repositories;
using AidManager.API.Shared.Infraestructure.Persistence.EFC.Configuration;
using AidManager.API.Shared.Infraestructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AidManager.API.ManageTasks.Infrastructure.Repositories;

public class TeamMemberRepository(AppDBContext context) : BaseRepository<ProjectTeamMembers>(context), ITeamMemberRepository
{
    public async Task<List<ProjectTeamMembers>> GetTeamMembers(int projectId)
    {
        
        return await Context.Set<ProjectTeamMembers>().Where(f => f.ProjectId == projectId).ToListAsync();
        
    }

    public async Task<List<ProjectTeamMembers>> GetProject(int userId)
    {
        return await Context.Set<ProjectTeamMembers>().Where(f => f.UserId == userId).ToListAsync();
        
    }
    
    public async Task<bool> Exists(int userId, int projectId)
    {
        return await Context.Set<ProjectTeamMembers>().AnyAsync(f => f.UserId == userId && f.ProjectId == projectId);
    }

   public async Task<bool> RemoveDeletedUser(int userId)
    {
        var user = await Context.Set<ProjectTeamMembers>().FirstOrDefaultAsync(f => f.UserId == userId);
        if (user != null)
        {
            Context.Set<ProjectTeamMembers>().Remove(user);
            await Context.SaveChangesAsync();
            return true;
        }
        return false;
    }
}