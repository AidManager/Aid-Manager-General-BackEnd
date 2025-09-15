using AidManager.API.ManageTasks.Domain.Model.Commands;
using AidManager.API.ManageTasks.Interfaces.REST.Resources;

namespace AidManager.API.ManageTasks.Interfaces.REST.Transform;

public class UpdateProjectStatusResourceFromResourceAssembler
{
    public static UpdateProjectStatusCommand ToCommandFromResource(int projectId, int id, UpdateProjectResource resource) =>
        new UpdateProjectStatusCommand(projectId, id);
}