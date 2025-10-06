using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.Collaborate.Domain.Model.Commands;
using AidManager.API.ManageTasks.Domain.Model.Aggregates;
using AidManager.API.ManageTasks.Domain.Model.Commands;
using AidManager.API.ManageTasks.Domain.Model.ValueObjects;

namespace AidManager.API.ManageTasks.Domain.Services;

public interface IProjectCommandService
{
    Task<(Project,List<User>)> Handle(CreateProjectCommand command);
    
    Task<(Project,List<User>)> Handle(AddProjectImageCommand command);
    
    Task<(Project,List<User>)> Handle(DeleteProjectCommand command);
    
    Task<(Project,List<User>)> Handle(UpdateProjectCommand command);
    
    Task<(Project,List<User>)> Handle(SaveProjectAsFavorite command);
    Task<(Project,List<User>)> Handle(RemoveProjectAsFavorite command);

    Task<(Project,List<User>)> Handle(UpdateRatingCommand command);
    
}