using AidManager.API.ManageTasks.Domain.Model.Aggregates;
using AidManager.API.Shared.Domain.Repositories;

namespace AidManager.API.ManageTasks.Domain.Repositories;

public interface ITeamMemberRepository: IBaseRepository<ProjectTeamMembers>
{
    Task<List<ProjectTeamMembers>> GetTeamMembers(int projectId);
    Task<List<ProjectTeamMembers>> GetProject(int userId);
    
    Task<bool> Exists(int userId, int projectId);
    
    Task<bool> RemoveDeletedUser(int userId);
}