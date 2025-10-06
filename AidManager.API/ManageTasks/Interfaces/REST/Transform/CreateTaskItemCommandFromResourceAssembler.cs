using AidManager.API.ManageTasks.Domain.Model.Commands;
using AidManager.API.ManageTasks.Interfaces.REST.Resources;

namespace AidManager.API.ManageTasks.Interfaces.REST.Transform;

public static class CreateTaskItemCommandFromResourceAssembler
{
    public static CreateTaskCommand ToCommandFromResource(CreateTaskItemResource resource, int projectId) =>
        new CreateTaskCommand(resource.Title, resource.Description, resource.DueDate, projectId , resource.State, resource.AssigneeId);
}