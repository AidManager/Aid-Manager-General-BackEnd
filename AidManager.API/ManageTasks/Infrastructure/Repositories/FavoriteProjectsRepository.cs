using AidManager.API.ManageTasks.Domain.Model.ValueObjects;
using AidManager.API.ManageTasks.Domain.Repositories;
using AidManager.API.Shared.Infraestructure.Persistence.EFC.Configuration;
using AidManager.API.Shared.Infraestructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AidManager.API.ManageTasks.Infrastructure.Repositories;

public class FavoriteProjectsRepository(AppDBContext context) : BaseRepository<FavoriteProjects>(context), IFavoriteProjects
{
    public async Task<List<FavoriteProjects>> GetFavoriteProjectsByUserIdAsync(int projectId)
    {
        return await Context.Set<FavoriteProjects>().Where(f => f.UserId == projectId).ToListAsync();
    }

    public async Task<FavoriteProjects?> GetFavoriteProjectsByProjectIdAndUserIdAsync(int userId, int projectId)
    {
        return await Context.Set<FavoriteProjects>().FirstOrDefaultAsync(f => f.UserId == userId && f.ProjectId == projectId);
    }
}