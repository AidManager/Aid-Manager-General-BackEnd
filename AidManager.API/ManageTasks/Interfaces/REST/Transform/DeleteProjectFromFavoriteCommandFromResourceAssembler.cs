using AidManager.API.ManageTasks.Domain.Model.Commands;
using AidManager.API.ManageTasks.Interfaces.REST.Resources;

namespace AidManager.API.ManageTasks.Interfaces.REST.Transform;

public static class DeleteProjectFromFavoriteCommandFromResourceAssembler
{
    public static RemoveProjectAsFavorite ToCommandFromResource(SaveProjectResource resource) =>
        new RemoveProjectAsFavorite(resource.UserId, resource.ProjectId);
    
}