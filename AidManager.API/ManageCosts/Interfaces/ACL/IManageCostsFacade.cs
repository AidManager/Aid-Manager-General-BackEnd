using AidManager.API.ManageCosts.Domain.Model.Aggregates;

namespace AidManager.API.ManageCosts.Interfaces.ACL;

public interface IManageCostsFacade
{
    Task<Analytics> CreateAnalytics(int projectId);

}