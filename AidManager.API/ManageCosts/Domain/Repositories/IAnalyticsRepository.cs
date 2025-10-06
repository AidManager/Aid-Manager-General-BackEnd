using AidManager.API.ManageCosts.Domain.Model.Aggregates;
using AidManager.API.Shared.Domain.Repositories;

namespace AidManager.API.ManageCosts.Domain.Repositories;

public interface IAnalyticsRepository : IBaseRepository<Analytics>
{
    Task<Analytics?> FindAnalyticsById(int id);
    Task<Analytics?> GetAnalyticsById(int id);
    Task<Analytics?> GetAnalyticsByProjectId(int projectId);
}