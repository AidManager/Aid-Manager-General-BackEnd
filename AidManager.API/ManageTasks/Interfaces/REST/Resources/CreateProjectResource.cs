namespace AidManager.API.ManageTasks.Interfaces.REST.Resources;

public record CreateProjectResource(
    string Name,
    string Description,
    List<string> ImageUrl,
    int CompanyId,
    DateOnly ProjectDate,
    TimeOnly ProjectTime,
    string ProjectLocation);
