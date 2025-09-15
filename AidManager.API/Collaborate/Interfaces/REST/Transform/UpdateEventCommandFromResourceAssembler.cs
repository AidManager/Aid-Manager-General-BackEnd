using AidManager.API.Collaborate.Domain.Model.Commands;
using AidManager.API.Collaborate.Interfaces.REST.Resources;

namespace AidManager.API.Collaborate.Interfaces.REST.Transform;

public static class UpdateEventCommandFromResourceAssembler
{
    public static EditEventCommand ToCommandFromResource(int id, UpdateEventResource resource)
    {
        return new EditEventCommand(id, resource.Name, resource.Location, resource.Description);
    }
}