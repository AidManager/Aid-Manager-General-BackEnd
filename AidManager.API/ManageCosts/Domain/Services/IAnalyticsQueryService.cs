using AidManager.API.ManageCosts.Domain.Model.Aggregates;
using AidManager.API.ManageCosts.Domain.Model.Queries;

namespace AidManager.API.ManageCosts.Domain.Services;

public interface IAnalyticsQueryService
{
    Task<Analytics?> Handle(GetAnalyticsByProjectId query);
    
    Task<Analytics?> Handle(GetAnalyticsById query);
    
    Task<List<Analytics?>> Handle(GetAnalyticsByCompanyId query);
    
}