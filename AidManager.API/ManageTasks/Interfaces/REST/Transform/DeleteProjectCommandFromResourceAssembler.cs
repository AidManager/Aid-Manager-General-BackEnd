using AidManager.API.ManageTasks.Domain.Model.Commands;
using AidManager.API.ManageTasks.Interfaces.REST.Resources;

namespace AidManager.API.ManageTasks.Interfaces.REST.Transform;

public class DeleteProjectCommandFromResourceAssembler
{
    public static DeleteProjectCommand ToCommandFromResource(int projectId) =>
        new DeleteProjectCommand(projectId);
}