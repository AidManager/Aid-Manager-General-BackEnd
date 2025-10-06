namespace AidManager.API.ManageTasks.Domain.Model.Aggregates;

public class ProjectTeamMembers
{
    public int UserId { get; set; }
    public int ProjectId { get; set; }
    
    public ProjectTeamMembers()
    {
        
    }
    
    public ProjectTeamMembers(int userId, int projectId)
    {
        UserId = userId;
        ProjectId = projectId;
    }
    
}