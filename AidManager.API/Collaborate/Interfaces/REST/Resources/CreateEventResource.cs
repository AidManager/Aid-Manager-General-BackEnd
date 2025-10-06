namespace AidManager.API.Collaborate.Interfaces.REST.Resources;

public record CreateEventResource(string Name, string Date, string Location, string Description, string Color, int ProjectId);