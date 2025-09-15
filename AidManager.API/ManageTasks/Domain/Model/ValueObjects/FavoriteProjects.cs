using AidManager.API.ManageTasks.Domain.Model.Commands;

namespace AidManager.API.ManageTasks.Domain.Model.ValueObjects;

public class FavoriteProjects
{
    public int UserId { get; set; }
    public int ProjectId { get; set; }
    
    public FavoriteProjects(SaveProjectAsFavorite command)
    {
        UserId = command.UserId;
        ProjectId = command.ProjectId;
    }
    
    public FavoriteProjects()
    {
        
    }
    
}