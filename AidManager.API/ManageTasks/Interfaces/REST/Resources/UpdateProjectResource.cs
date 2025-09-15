namespace AidManager.API.ManageTasks.Interfaces.REST.Resources;

public record UpdateProjectResource(string Name,
    string Description,
    List<string> ImageUrl,
    int CompanyId,
    DateOnly ProjectDate,
    TimeOnly ProjectTime,
    string ProjectLocation);