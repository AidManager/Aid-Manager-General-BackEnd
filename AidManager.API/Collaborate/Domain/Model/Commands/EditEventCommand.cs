namespace AidManager.API.Collaborate.Domain.Model.Commands;

public record EditEventCommand(int Id, string Name, string Location, string Description);