using AidManager.API.ManageTasks.Domain.Model.Aggregates;

namespace AidManager.API.ManageTasks.Interfaces.ACL;

public interface IManageTasksFacade
{
    Task<IEnumerable<Project>> GetProjectsByCompany(int companyId);

}