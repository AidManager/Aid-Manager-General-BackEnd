using AidManager.API.ManageTasks.Domain.Model.Commands;
using AidManager.API.ManageTasks.Interfaces.REST.Resources;

namespace AidManager.API.ManageTasks.Interfaces.REST.Transform;

public static class UpdateTaskItemCommandFromResourceAssembler
{
    public static UpdateTaskCommand ToCommandFromResource(UpdateTaskItemResource resource, int taskId ,int projectId) =>
        new UpdateTaskCommand(taskId,resource.Title, resource.Description, resource.DueDate, resource.State, resource.assigneeId, projectId);

}