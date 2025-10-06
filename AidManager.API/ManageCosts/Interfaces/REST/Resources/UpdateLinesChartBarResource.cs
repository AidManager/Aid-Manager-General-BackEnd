using AidManager.API.ManageCosts.Domain.Model.Entities;

namespace AidManager.API.ManageCosts.Interfaces.REST.Resources;

public record UpdateLinesChartBarResource(
    List<LineChartDataResource> Lines
    );