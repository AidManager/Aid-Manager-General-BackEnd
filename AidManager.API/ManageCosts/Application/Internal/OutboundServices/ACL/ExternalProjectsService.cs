using AidManager.API.ManageTasks.Domain.Model.Aggregates;
using AidManager.API.ManageTasks.Interfaces.ACL;

namespace AidManager.API.ManageCosts.Application.Internal.OutboundServices.ACL;

public class ExternalProjectsService(IManageTasksFacade manageTasksFacade)
{
    public async Task<IEnumerable<Project>> GetProjectsByCompany(int companyId)
    {
        return await manageTasksFacade.GetProjectsByCompany(companyId);
    }
    
}
