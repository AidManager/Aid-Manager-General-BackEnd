using AidManager.API.ManageCosts.Domain.Model.Entities;

namespace AidManager.API.ManageCosts.Domain.Model.Commands;

public record UpdateBarDataPaymentsCommand(
    int Id,
    List<BarData> BarData
    );