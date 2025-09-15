namespace AidManager.API.Collaborate.Interfaces.REST.Resources;

public record GetEventResource(int Id, string Name, string Date, string Location, string Description, string Color, int ProjectId);