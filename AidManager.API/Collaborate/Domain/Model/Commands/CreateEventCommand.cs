namespace AidManager.API.Collaborate.Domain.Model.Commands;

public record CreateEventCommand(string Name, string Date, string Location, string Description, string Color, int ProjectId);