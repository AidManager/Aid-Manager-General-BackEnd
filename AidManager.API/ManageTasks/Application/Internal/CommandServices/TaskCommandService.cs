using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.ManageTasks.Application.Internal.OutboundServices;
using AidManager.API.ManageTasks.Application.Internal.OutboundServices.ACL;
using AidManager.API.ManageTasks.Domain.Model.Aggregates;
using AidManager.API.ManageTasks.Domain.Model.Commands;
using AidManager.API.ManageTasks.Domain.Repositories;
using AidManager.API.ManageTasks.Domain.Services;
using AidManager.API.Shared.Domain.Repositories;

namespace AidManager.API.ManageTasks.Application.Internal.CommandServices;

public class TaskCommandService(ITeamMemberRepository teamMemberRepository,ITaskEventHandlerService eventHandlerService,ITaskRepository taskRepository, IUnitOfWork unitOfWork, IProjectRepository projectRepository, ExternalUserService externalUserService) : ITaskCommandService
{
    public async Task<(TaskItem, User)> Handle(CreateTaskCommand command)
    {
        try
        {
            bool exists = await projectRepository.ExistsProject(command.ProjectId);

            if (!exists)
            {
                throw new Exception($"Project with id {command.ProjectId} does not exist.");
            }
            
            var user = await externalUserService.GetUserById(command.AssigneeId);
            if (user.Role == 0)
            {
                throw new Exception($"Cant assign task to a manager user.");
            }
            
            var task = new TaskItem(command);

            if (!await teamMemberRepository.Exists(command.AssigneeId, command.ProjectId))
            {
                await eventHandlerService.HandleAddTeamMember(new AddTeamMemberCommand(command.AssigneeId, command.ProjectId));
            }
            await taskRepository.AddAsync(task);
            await unitOfWork.CompleteAsync();
            return (task, user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
    
    public async Task<(TaskItem, User)> Handle(UpdateTaskCommand command)
    {
        
        bool exists = await projectRepository.ExistsProject(command.ProjectId);

        if (!exists)
        {
            throw new Exception($"Project with id {command.ProjectId} does not exist.");
        }
        
        
        var user = await externalUserService.GetUserById(command.AssigneeId);
        if (user is null || user.FirstName == "Deleted")
        {
            await eventHandlerService.HandleRemoveTeamMember(command.AssigneeId, command.ProjectId);
            throw new Exception("User was Deleted");
        }
        
        var task = await taskRepository.GetTaskById(command.Id);
        if (task is null) throw new Exception("Task not found");

        var taskList = await taskRepository.GetTasksByUserId(task.AssigneeId);
        var taskCount = 0;

        foreach (var taskItem in taskList)
        {
            if (taskItem.ProjectId == command.ProjectId)
            {
                taskCount++;
            }
            
        }
        if (taskCount == 1)
        { 
            await eventHandlerService.HandleUpdateTeamMember(command.AssigneeId, command.ProjectId, task.AssigneeId);
        }
        else
        {
            if (!await teamMemberRepository.Exists(command.AssigneeId, command.ProjectId))
            {
                await eventHandlerService.HandleAddTeamMember(new AddTeamMemberCommand(command.AssigneeId, command.ProjectId));
            }
        }

        
        task.UpdateTask(command);
        await taskRepository.Update(task);
        await unitOfWork.CompleteAsync();
        return (task, user);
    }
    
    public async Task<(TaskItem,User)> Handle(DeleteTaskCommand command)
    {
        
        bool exists = await projectRepository.ExistsProject(command.ProjectId);
        

        if (!exists)
        {
            throw new Exception($"Project with id {command.ProjectId} does not exist.");
        }
        
        var task = await taskRepository.GetTaskById(command.Id);
        if (task is null) throw new Exception("Task not found");
        var user = await externalUserService.GetUserById(task.AssigneeId);
        await eventHandlerService.HandleRemoveTeamMember(task.AssigneeId, command.ProjectId);
        await taskRepository.Remove(task);
        await unitOfWork.CompleteAsync();
        return (task, user);
    }
    
}