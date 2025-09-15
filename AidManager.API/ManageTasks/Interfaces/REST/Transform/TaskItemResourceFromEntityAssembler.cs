using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.ManageTasks.Domain.Model.Aggregates;
using AidManager.API.ManageTasks.Interfaces.REST.Resources;

namespace AidManager.API.ManageTasks.Interfaces.REST.Transform;

public static class TaskItemResourceFromEntityAssembler
{
    public static TaskItemResource ToResourceFromEntity(TaskItem entity, User user) =>
        new TaskItemResource(entity.Id, entity.Title, entity.Description, entity.CreatedAt, entity.DueDate,
            entity.State, entity.AssigneeId, user.FirstName + " " + user.LastName,user.ProfileImg, entity.ProjectId);
}