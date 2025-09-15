using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.ManageTasks.Domain.Model.Aggregates;
using AidManager.API.ManageTasks.Domain.Model.Commands;
using AidManager.API.ManageTasks.Domain.Model.Queries;

namespace AidManager.API.ManageTasks.Domain.Services;

public interface ITaskQueryService
{
    Task<List<(TaskItem, User)>> Handle(GetTasksByUserId query);
    Task<(TaskItem, User)> Handle(GetTaskByIdQuery query);
    Task<List<(TaskItem, User)>> Handle(GetTasksByProjectIdQuery query);
    Task<List<(TaskItem, User)>> Handle(GetTasksByCompanyId query);
    Task<List<(TaskItem, User)>> Handle(GetAllTasksByUserIdByCompanyId query);
    Task<List<(TaskItem, User)>> Handle(GetTasksByUserIdAndProjectId query);

}