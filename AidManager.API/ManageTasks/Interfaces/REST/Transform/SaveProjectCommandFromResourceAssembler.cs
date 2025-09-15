using AidManager.API.ManageTasks.Domain.Model.Commands;
using AidManager.API.ManageTasks.Interfaces.REST.Resources;

namespace AidManager.API.ManageTasks.Interfaces.REST.Transform;

public static class SaveProjectCommandFromResourceAssembler
{
    public static SaveProjectAsFavorite ToCommandFromResource(SaveProjectResource resource) =>
        new SaveProjectAsFavorite(resource.UserId, resource.ProjectId);
}