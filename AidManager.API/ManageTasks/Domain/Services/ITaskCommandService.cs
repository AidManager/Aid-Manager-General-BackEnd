using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.ManageTasks.Domain.Model.Aggregates;
using AidManager.API.ManageTasks.Domain.Model.Commands;

namespace AidManager.API.ManageTasks.Domain.Services;

public interface ITaskCommandService
{
    Task<(TaskItem, User)> Handle(CreateTaskCommand command);
    Task<(TaskItem, User)> Handle(UpdateTaskCommand command);
    Task<(TaskItem,User)> Handle(DeleteTaskCommand command);
}