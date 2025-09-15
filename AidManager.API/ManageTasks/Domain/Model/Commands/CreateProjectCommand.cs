namespace AidManager.API.ManageTasks.Domain.Model.Commands;

public record CreateProjectCommand( string Name, string Description, List<string> ImageUrl, int CompanyId, string ProjectDate, string ProjectTime, string ProjectLocation);