namespace AidManager.API.ManageTasks.Domain.Model.Commands;

public record UpdateProjectCommand(int ProjectId, string Name, string Description, List<string> ImageUrl, int CompanyId, string ProjectDate, string ProjectTime, string ProjectLocation);