using AidManager.API.Collaborate.Domain.Model.Commands;
using AidManager.API.Collaborate.Interfaces.REST.Resources;

namespace AidManager.API.Collaborate.Interfaces.REST.Transform;

public static class CreateEventCommandFromResourceAssembler
{
    public static CreateEventCommand ToCommandFromResource(CreateEventResource resource)
    {
        return new CreateEventCommand(resource.Name, resource.Date, resource.Location, resource.Description, resource.Color, resource.ProjectId);
}
}