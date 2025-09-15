using AidManager.API.Collaborate.Domain.Model.Entities;
using AidManager.API.Collaborate.Interfaces.REST.Resources;

namespace AidManager.API.Collaborate.Interfaces.REST.Transform;

public static class EventResourceFromEntityAssembler
{
    public static GetEventResource ToResourceFromEntity(Event eventEntity)
    {
        return new GetEventResource
        (
            eventEntity.Id,
            eventEntity.Name,
            eventEntity.Date,
            eventEntity.Location,
            eventEntity.Description,
            eventEntity.Color,
            eventEntity.ProjectId
        );
    }
}