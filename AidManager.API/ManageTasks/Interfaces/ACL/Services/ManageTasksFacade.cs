using AidManager.API.ManageTasks.Domain.Model.Aggregates;
using AidManager.API.ManageTasks.Domain.Model.Queries;
using AidManager.API.ManageTasks.Domain.Services;

namespace AidManager.API.ManageTasks.Interfaces.ACL.Services;

public class ManageTasksFacade(IProjectQueryService queryService) : IManageTasksFacade
{
    public async Task<IEnumerable<Project>> GetProjectsByCompany(int companyId)
    {
        var getAllProjects = new GetAllProjectsQuery(companyId);
        var result = await queryService.Handle(getAllProjects);
        
        return result.Select(x => x.Item1);
        
    }
}