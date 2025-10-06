using AidManager.API.ManageCosts.Domain.Model.Entities;

namespace AidManager.API.ManageCosts.Domain.Model.Commands;

public record UpdateLinesChartBarCommand(
    int Id,
    List<LineChartData> Lines
);