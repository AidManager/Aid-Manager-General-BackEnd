using AidManager.API.ManageCosts.Domain.Model.Aggregates;
using AidManager.API.ManageCosts.Domain.Model.Commands;
using AidManager.API.ManageCosts.Domain.Model.Entities;
using AidManager.API.ManageCosts.Domain.Services;

namespace AidManager.API.ManageCosts.Interfaces.ACL.Services;

public class ManageCostsFacade(IAnalyticsCommandService commandService) : IManageCostsFacade
{
    public async Task<Analytics> CreateAnalytics(int projectId)
    {
        List<LineChartData> linesChartBarData;
        List<BarData> barData;
        List<double> progressbar;
        List<double> status;
        List<double> tasks;
        
        linesChartBarData = new List<LineChartData>();
        linesChartBarData.Add(new LineChartData());
        barData = new List<BarData>();
        barData.Add(new BarData());
        progressbar = new List<double>();
        progressbar.Add(0);
        status = new List<double>();
        status.Add(0);
        tasks = new List<double>();
        tasks.Add(0);
        
            
        var analyticsCommand = new CreateAnalyticsCommand(projectId, linesChartBarData, barData, progressbar, status, tasks);

        var analytics = await commandService.Handle(analyticsCommand);

        return analytics ?? new Analytics();

    }
}