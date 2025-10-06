namespace AidManager.API.ManageCosts.Domain.Model.Commands;

public record UpdateAnalyticTasksCommand(
    int Id,
    List<double> Tasks
    );