using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.Collaborate.Domain.Model.Commands;
using AidManager.API.ManageTasks.Application.Internal.OutboundServices;
using AidManager.API.ManageTasks.Application.Internal.OutboundServices.ACL;
using AidManager.API.ManageTasks.Domain.Model.Aggregates;
using AidManager.API.ManageTasks.Domain.Model.Commands;
using AidManager.API.ManageTasks.Domain.Model.ValueObjects;
using AidManager.API.ManageTasks.Domain.Repositories;
using AidManager.API.ManageTasks.Domain.Services;
using AidManager.API.Shared.Domain.Repositories;

namespace AidManager.API.ManageTasks.Application.Internal.CommandServices;

public class ProjectCommandService(ITeamMemberRepository teamMemberRepository,ITaskRepository taskRepository,IProjectRepository projectRepository, IUnitOfWork unitOfWork,IFavoriteProjects favoriteProjects, ExternalUserService externalUserService): IProjectCommandService
{
    public async Task<(Project,List<User>)> Handle(CreateProjectCommand command)
    {
        try {
            
            if(command.CompanyId == 0)
            {
                throw new Exception("Company Id is required");
            }
            
            var existsByName = await projectRepository.ExistsByName(command.Name);
            
            if (existsByName)
            {
                throw new Exception($"Project with name {command.Name} already exists.");
            }
            
            var project = new Project(command);
            
                await projectRepository.AddAsync(project);
                Console.WriteLine("Project added: " + project.Id);
                await externalUserService.CreateAnalytics(project.Id);
                await unitOfWork.CompleteAsync();
                var team = new List<User>();
                return (project, team);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    public async Task<(Project,List<User>)> Handle(AddProjectImageCommand command)
    {
        var project = await projectRepository.GetProjectById(command.ProjectId);
        if (project == null)
        {
            throw new Exception("Project not Found");
        }
        project.AddImage(command);
        
        var team = new List<User>();
        
        var teams = await teamMemberRepository.GetTeamMembers(project.Id);
        foreach (var teamMember in teams)
        {
            var user = await externalUserService.GetUserById(teamMember.UserId);
            if (user is null || user.FirstName == "Deleted")
            {
                
                await teamMemberRepository.RemoveDeletedUser(teamMember.UserId);
                continue;
            }
            team.Add(user);
        }
        await projectRepository.Update(project);
        await unitOfWork.CompleteAsync();
        return (project, team);
        
    }

    public async Task<(Project,List<User>)> Handle(DeleteProjectCommand command)
    {
        var project = await projectRepository.GetProjectById(command.ProjectId);
        if (project == null)
        {
            throw new Exception("Project not Found");
        }
        
        var team = new List<User>();
        var teams = await teamMemberRepository.GetTeamMembers(project.Id);
        foreach (var teamMember in teams)
        {
            var user = await externalUserService.GetUserById(teamMember.UserId);
            if (user is null || user.FirstName == "Deleted")
            {
                await teamMemberRepository.RemoveDeletedUser(teamMember.UserId);
                continue;
            }
            
            var userTasks= await taskRepository.GetTasksByUserId(teamMember.UserId);
            foreach (var task in userTasks)
            {
                await taskRepository.Remove(task);
            }
            team.Add(user);
        }
        
        await projectRepository.Remove(project);
        await unitOfWork.CompleteAsync();
        return (project, team);
    }

    public async Task<(Project,List<User>)> Handle(UpdateProjectCommand command)
    {
        var project = await projectRepository.GetProjectById(command.ProjectId);
        if (project == null)
        {
            throw new Exception("Project not Found");
        }
        
        project.UpdateProject(command);
        
        var team = new List<User>();
        var teams = await teamMemberRepository.GetTeamMembers(project.Id);
        foreach (var teamMember in teams)
        {
            var user = await externalUserService.GetUserById(teamMember.UserId);
            if (user is null || user.FirstName == "Deleted")
            {
                
                await teamMemberRepository.RemoveDeletedUser(teamMember.UserId);
                continue;
            }
            team.Add(user);
        }
        await projectRepository.Update(project);
        await unitOfWork.CompleteAsync();
        return (project, team);
    }

    public async Task<(Project, List<User>)> Handle(SaveProjectAsFavorite command)
    {
        var project = await projectRepository.GetProjectById(command.ProjectId);
        if (project == null)
        {
            throw new Exception("Project not Found");
        }
        var team = new List<User>();
        var teams = await teamMemberRepository.GetTeamMembers(project.Id);

        foreach (var teamMember in teams)
        {
            var user = await externalUserService.GetUserById(teamMember.UserId);
            if (user is null || user.FirstName == "Deleted")
            {
                
                await teamMemberRepository.RemoveDeletedUser(teamMember.UserId);
                continue;
            }
            team.Add(user);
        }

        var saved = new FavoriteProjects(command);
        
        var exists = await favoriteProjects.GetFavoriteProjectsByProjectIdAndUserIdAsync(command.ProjectId, command.UserId);

        if (exists != null)
        {
            return (project, team);
        }
        
        await favoriteProjects.AddAsync(saved);
        await unitOfWork.CompleteAsync();
        return (project, team);
    }

    public async Task<(Project, List<User>)> Handle(RemoveProjectAsFavorite command)
    {
        var saved = await favoriteProjects.GetFavoriteProjectsByProjectIdAndUserIdAsync(command.UserId, command.ProjectId);
        if (saved == null)
        {
            throw new Exception("Project is not saved as favorite.");
        }
        var project = await projectRepository.GetProjectById(command.ProjectId);
        if (project == null)
        {
            await favoriteProjects.Remove(saved);
            throw new Exception("This project was deleted.");
        }
        var team = new List<User>();
        var teams = await teamMemberRepository.GetTeamMembers(project.Id);

        foreach (var teamMember in teams)
        {
            var user = await externalUserService.GetUserById(teamMember.UserId);
            if (user is null || user.FirstName == "Deleted")
            {
                
                await teamMemberRepository.RemoveDeletedUser(teamMember.UserId);
                continue;
            }
            team.Add(user);
        }
        
        await favoriteProjects.Remove(saved);
        await unitOfWork.CompleteAsync();
        return (project, team);    
    }

    public async Task<(Project, List<User>)> Handle(UpdateRatingCommand command)
    {
        if (command.Rating < 1 || command.Rating > 5)
        {
            throw new Exception("Rating not valid must be between 1 - 5 >:V");
        }
        var project = await projectRepository.GetProjectById(command.ProjectId);
        if (project == null)
        {
            throw new Exception("Project not Found");
        }
        
        var rating = command.Rating;
        project.UpdateRating(rating);
        
        var team = new List<User>();
        var teams = await teamMemberRepository.GetTeamMembers(project.Id);

        foreach (var teamMember in teams)
        {
            var user = await externalUserService.GetUserById(teamMember.UserId);
            if (user is null || user.FirstName == "Deleted")
            {
                await teamMemberRepository.RemoveDeletedUser(teamMember.UserId);
                continue;
            }
            team.Add(user);
        }
        await projectRepository.Update(project);
        await unitOfWork.CompleteAsync();
        return (project, team);
    }
}