using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.ManageTasks.Domain.Model.Aggregates;
using AidManager.API.Shared.Domain.Repositories;

namespace AidManager.API.ManageTasks.Domain.Repositories;

public interface IProjectRepository : IBaseRepository<Project>
{
    Task<bool> ExistsProject(int projectId);
    Task<bool> ExistsByName(string name);
    Task<List<Project?>> GetProjectsByCompanyId(int companyId);
    Task<Project?> GetProjectById(int projectId);
    Task<List<User>> GetTeamMembers(int companyId);
    
    Task<List<Project?>> GetProjectsByUserId(int userId);
    
}