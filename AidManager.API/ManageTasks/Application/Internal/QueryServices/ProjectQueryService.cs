using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.ManageTasks.Application.Internal.OutboundServices.ACL;
using AidManager.API.ManageTasks.Domain.Model.Aggregates;
using AidManager.API.ManageTasks.Domain.Model.Commands;
using AidManager.API.ManageTasks.Domain.Model.Queries;
using AidManager.API.ManageTasks.Domain.Repositories;
using AidManager.API.ManageTasks.Domain.Services;
using AidManager.API.Shared.Domain.Repositories;

namespace AidManager.API.ManageTasks.Application.Internal.QueryServices;

public class ProjectQueryService(IUnitOfWork unitOfWork,ITeamMemberRepository teamMemberRepository,IFavoriteProjects favoriteProjects,IProjectRepository projectRepository, ExternalUserService externalUserService) :IProjectQueryService
{
    public async Task<IEnumerable<(Project,List<User>)>> Handle(GetAllProjectsQuery query)
    {
        
       var projectList = await projectRepository.GetProjectsByCompanyId(query.CompanyId);
       var projectUserList = new List<(Project, List<User>)>();
       
         foreach (var project in projectList)
         {
             if (project == null)
             {
                    continue;
             } 
             var team = new List<User>();
             var teams = await teamMemberRepository.GetTeamMembers(project.Id);
              foreach (var teamMember in teams)
              {
                  var user = await externalUserService.GetUserById(teamMember.UserId);
                if (user is { FirstName: "Deleted", Age: 0 })
                {
                    await teamMemberRepository.RemoveDeletedUser(teamMember.UserId);
                    await unitOfWork.CompleteAsync();
                    continue;
                }
                team.Add(user);
              }
              projectUserList.Add((project, team));
              
         }
       
         return projectUserList;
       
    }
    
    public async Task<(Project? project, List<User> team)> Handle(GetProjectByIdQuery query)
    {
        var project = await projectRepository.GetProjectById(query.id);
        var team = new List<User>();
        
        if (project == null)
        {
            throw new Exception("Project not found or was Deleted");
        }
        var teams = await teamMemberRepository.GetTeamMembers(project.Id);

        foreach (var teamMember in teams)
        {
            var user = await externalUserService.GetUserById(teamMember.UserId);
            if (user is { FirstName: "Deleted", Age: 0 })
            {
                await teamMemberRepository.RemoveDeletedUser(teamMember.UserId);
                await unitOfWork.CompleteAsync();
                continue;
            }
            team.Add(user);
        }
        
        
        return (project, team);
        
    }

    public async Task<IEnumerable<User>> Handle(GetAllTeamMembers query)
    {
        var project = await projectRepository.GetProjectById(query.ProjectId);
        var team = new List<User>();
        
        if (project == null)
        {
            throw new Exception("Project not found or was Deleted");
        }
        var teams = await teamMemberRepository.GetTeamMembers(project.Id);

        foreach (var teamMember in teams)
        {
            var user = await externalUserService.GetUserById(teamMember.UserId);
            if (user is { FirstName: "Deleted", Age: 0 })
            {
                await teamMemberRepository.RemoveDeletedUser(teamMember.UserId);
                await unitOfWork.CompleteAsync();
                continue;
            }
            team.Add(user);
        }
        
        return team;
    }

    public async Task<IEnumerable<(Project, List<User>)>> Handle(GetAllProjectsByUserIdQuery query)
    {
        var projectList = await teamMemberRepository.GetProject(query.UserId);
        var projectUserList = new List<(Project, List<User>)>();



        foreach (var project in projectList)
        {
            var team = new List<User>();
            var teams = await teamMemberRepository.GetTeamMembers(project.ProjectId);

            foreach (var teamMember in teams)
            {
                var user = await externalUserService.GetUserById(teamMember.UserId);
                if (user is { FirstName: "Deleted", Age: 0 })
                {
                    await teamMemberRepository.RemoveDeletedUser(teamMember.UserId);
                    await unitOfWork.CompleteAsync();
                    continue;
                }
                team.Add(user);
            }
            var projectEntity = await projectRepository.GetProjectById(project.ProjectId);

            if (projectEntity == null)
            {
                continue;
            }
            
            projectUserList.Add((projectEntity, team));
        }
       
        return projectUserList;
    }

    public async Task<IEnumerable<(Project, List<User>)>> Handle(GetFavoriteProjectsByUserId query)
    {
       var projectsList = await favoriteProjects.GetFavoriteProjectsByUserIdAsync(query.UserId);
       var projectUserList = new List<(Project, List<User>)>();
       var team = new List<User>();
       
         foreach (var project in projectsList)
         {
                var projectEntity = await projectRepository.GetProjectById(project.ProjectId);
                if (projectEntity == null)
                {
                    var delete = await favoriteProjects.GetFavoriteProjectsByProjectIdAndUserIdAsync(query.UserId, project.ProjectId);
                    if (delete == null)
                    {
                        continue;
                    }
                    await favoriteProjects.Remove(delete);
                    continue;
                }
                var teams = await teamMemberRepository.GetTeamMembers(project.ProjectId);

                foreach (var teamMember in teams)
                {
                    var user = await externalUserService.GetUserById(teamMember.UserId);
                    if (user is { FirstName: "Deleted", Age: 0 })
                    {
                        await teamMemberRepository.RemoveDeletedUser(teamMember.UserId);
                        await unitOfWork.CompleteAsync();

                        
                        continue;
                    }
                    team.Add(user);
                }
                projectUserList.Add((projectEntity, team));
         }
         
         return projectUserList;
       
    }
}