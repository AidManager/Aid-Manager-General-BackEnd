using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.ManageTasks.Application.Internal.OutboundServices.ACL;
using AidManager.API.ManageTasks.Domain.Model.Aggregates;
using AidManager.API.ManageTasks.Domain.Model.Commands;
using AidManager.API.ManageTasks.Domain.Model.Queries;
using AidManager.API.ManageTasks.Domain.Repositories;
using AidManager.API.ManageTasks.Domain.Services;

namespace AidManager.API.ManageTasks.Application.Internal.QueryServices;

public class TaskQueryService(ITaskRepository taskRepository, ExternalUserService external, IProjectQueryService projectQuery) : ITaskQueryService
{

    public async Task<(TaskItem, User)> Handle(GetTaskByIdQuery query)
    {
        var taskItem = await taskRepository.GetTaskById(query.Id);
        if (taskItem is null)
        {
            throw new Exception("Error: Task not found");
        }

        var user = await external.GetUserById(taskItem.AssigneeId);

        return (taskItem, user);

    }

    public async Task<List<(TaskItem, User)>> Handle(GetTasksByProjectIdQuery query)
    {
        var taskList = await taskRepository.GetTasksByProjectId(query.ProjectId);
        var taskUserList = new List<(TaskItem, User)>();
        foreach (var task in taskList)
        {
            var user = await external.GetUserById(task.AssigneeId);
            taskUserList.Add((task, user));
        }
        return taskUserList;
    }

    public async Task<List<(TaskItem, User)>> Handle(GetTasksByCompanyId query)
    {
        try
        {
            var getAllProjectsQuery = new GetAllProjectsQuery(query.CompanyId);
            var projects = await projectQuery.Handle(getAllProjectsQuery);

            var tasksList = new List<TaskItem>();

            foreach (var project in projects)
            {
                var tasks = await taskRepository.GetTasksByProjectId(project.Item1.Id);
                tasksList.AddRange(tasks);
            }

            var taskUserList = new List<(TaskItem, User)>();
            foreach (var task in tasksList)
            {
                var user = await external.GetUserById(task.AssigneeId);
                taskUserList.Add((task, user));
            }
            return taskUserList;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }

    public async Task<List<(TaskItem, User)>> Handle(GetAllTasksByUserIdByCompanyId query)
    {
        try
        {
            var getAllProjectsQuery = new GetAllProjectsQuery(query.CompanyId);
            var projects = await projectQuery.Handle(getAllProjectsQuery);

            var tasksList = new List<TaskItem>();

            foreach (var project in projects)
            {
                var tasks = await taskRepository.GetTasksByProjectId(project.Item1.Id);
                tasksList.AddRange(tasks);
            }

            var taskUserList = new List<(TaskItem, User)>();
            foreach (var task in tasksList)
            {
                if (task.AssigneeId == query.UserId)
                {
                    var user = await external.GetUserById(task.AssigneeId);
                    taskUserList.Add((task, user));
                }
            }

            return taskUserList;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<(TaskItem, User)>> Handle(GetTasksByUserId query)
    {
        var taskList = await taskRepository.GetTasksByUserId(query.UserId);
        var taskUserList = new List<(TaskItem, User)>();
        foreach (var task in taskList)
        {
            var user = await external.GetUserById(task.AssigneeId);
            taskUserList.Add((task, user));
        }
        return taskUserList;
    }

    public async Task<List<(TaskItem, User)>> Handle(GetTasksByUserIdAndProjectId query)
    {
        var taskList = await taskRepository.GetTasksByUserIdAndProjectId(query.userId, query.ProjectId);
        var taskUserList = new List<(TaskItem, User)>();
        foreach (var task in taskList)
        {
            var user = await external.GetUserById(task.AssigneeId);
            taskUserList.Add((task, user));
        }
        return taskUserList;
    }
    
}