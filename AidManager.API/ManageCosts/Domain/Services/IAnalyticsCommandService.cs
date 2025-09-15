using AidManager.API.ManageCosts.Domain.Model.Aggregates;
using AidManager.API.ManageCosts.Domain.Model.Commands;

namespace AidManager.API.ManageCosts.Domain.Services;

public interface IAnalyticsCommandService
{
    Task<Analytics?> Handle(CreateAnalyticsCommand command);
    
    Task<Analytics?> Handle(UpdateBarDataPaymentsCommand command);
    
    Task<Analytics?> Handle(UpdateAnalyticProgressbarCommand command);
    
    Task<Analytics?> Handle(UpdateAnalyticStatusCommand command);
    
    Task<Analytics?> Handle(UpdateLinesChartBarCommand command);
    
    Task<Analytics?> Handle(UpdateAnalyticTasksCommand command);
}