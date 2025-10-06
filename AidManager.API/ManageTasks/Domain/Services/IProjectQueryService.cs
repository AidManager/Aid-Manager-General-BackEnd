using System.Collections;
using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.ManageTasks.Domain.Model.Aggregates;
using AidManager.API.ManageTasks.Domain.Model.Commands;
using AidManager.API.ManageTasks.Domain.Model.Queries;

namespace AidManager.API.ManageTasks.Domain.Services;

public interface IProjectQueryService
{
    Task<IEnumerable<(Project, List<User>)>> Handle(GetAllProjectsQuery query);
    
    Task<(Project? project, List<User> team)> Handle(GetProjectByIdQuery query);
    
    Task<IEnumerable<User>> Handle(GetAllTeamMembers query);

    Task<IEnumerable<(Project, List<User>)>> Handle(GetAllProjectsByUserIdQuery query);

    Task<IEnumerable<(Project, List<User>)>> Handle(GetFavoriteProjectsByUserId query);
    
}