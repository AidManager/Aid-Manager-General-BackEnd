using AidManager.API.ManageCosts.Application.Internal.OutboundServices.ACL;
using AidManager.API.ManageCosts.Domain.Model.Aggregates;
using AidManager.API.ManageCosts.Domain.Model.Queries;
using AidManager.API.ManageCosts.Domain.Repositories;
using AidManager.API.ManageCosts.Domain.Services;
using AidManager.API.ManageTasks.Domain.Model.Aggregates;
using AidManager.API.Shared.Domain.Repositories;

namespace AidManager.API.ManageCosts.Application.Internal.QueryServices;

public class AnalyticsQueryService(
    IAnalyticsRepository analyticsRepository,
    IUnitOfWork unitOfWork,
    ExternalProjectsService analyticsService
    ) : IAnalyticsQueryService
{
        
    public async Task<Analytics?> Handle(GetAnalyticsByProjectId query)
    {
        try
        {
            return await analyticsRepository.GetAnalyticsByProjectId(query.ProjectId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<Analytics?> Handle(GetAnalyticsById query)
    {
        try
        {
            return await analyticsRepository.GetAnalyticsById(query.Id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<Analytics?>> Handle(GetAnalyticsByCompanyId query)
    {
        try
        {
            var projects = await analyticsService.GetProjectsByCompany(query.CompanyId);

            var analyticsTasks = new List<Analytics?>();
            
            foreach (var project in projects)
            {
                var analytics = await analyticsRepository.GetAnalyticsByProjectId(project.Id);
                analyticsTasks.Add(analytics);
            }

            return analyticsTasks;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}