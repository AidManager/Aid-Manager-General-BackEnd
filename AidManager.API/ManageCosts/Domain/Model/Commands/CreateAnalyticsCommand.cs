using AidManager.API.ManageCosts.Domain.Model.Entities;

namespace AidManager.API.ManageCosts.Domain.Model.Commands;

public record CreateAnalyticsCommand(
    int ProjectId,
    List<LineChartData> linesChartBarData,
    List<BarData> barData,
    List<double> progressbar,
    List<double> status,
    List<double> tasks
);