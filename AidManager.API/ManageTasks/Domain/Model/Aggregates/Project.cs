using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.Collaborate.Domain.Model.Commands;
using AidManager.API.ManageTasks.Domain.Model.Commands;
using AidManager.API.ManageTasks.Domain.Model.ValueObjects;

namespace AidManager.API.ManageTasks.Domain.Model.Aggregates;

public class Project
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public double Rating { get; set; }

    public List<ProjectImage> ImageUrl { get; set; }
    
    public List<User> TeamMembers { get; set; }
    
    public int CompanyId { get; set; }
    
    public DateOnly ProjectDate { get; set; }
    
    public TimeOnly ProjectTime { get; set; }
    
    public string ProjectLocation { get; set; }
    
    public DateOnly AuditDate { get; set; }


    public Project()
    {
        ImageUrl = new List<ProjectImage>(); // Initialize the list
        TeamMembers = new List<User>(); // Initialize the list
        AuditDate = DateOnly.FromDateTime(DateTime.Now);
    }
    public Project(CreateProjectCommand command)
    {
        this.Name = command.Name;
        this.Description = command.Description;
        this.ImageUrl = command.ImageUrl?.ConvertAll(url => new ProjectImage { Url = url }) ?? new List<ProjectImage>();
        this.CompanyId = command.CompanyId;
        this.ProjectDate = DateOnly.Parse(command.ProjectDate);
        this.ProjectTime = TimeOnly.Parse(command.ProjectTime);
        this.ProjectLocation = command.ProjectLocation;
        this.Rating = 0.00;
        
        this.AuditDate = DateOnly.FromDateTime(DateTime.Now);
    }
    
    public void AddImage(AddProjectImageCommand command)
    {
        var images = command.ImagesUrl?.ConvertAll(url => new ProjectImage { Url = url }) ?? [];
        ImageUrl.AddRange(images);
    }
    
    public void UpdateProject(UpdateProjectCommand command)
    {
        this.Name = command.Name;
        this.Description = command.Description;
        this.ImageUrl = command.ImageUrl?.ConvertAll(url => new ProjectImage { Url = url }) ?? new List<ProjectImage>();
        this.CompanyId = command.CompanyId;
        this.ProjectDate = DateOnly.Parse(command.ProjectDate);
        this.ProjectTime = TimeOnly.Parse(command.ProjectTime);
        this.ProjectLocation = command.ProjectLocation;
    }
    
    public void AddTeamMember(User user)
    {
        if (TeamMembers.All(tm => tm.Id != user.Id))
        {
            TeamMembers.Add(user);
        }
    }
    
    public void UpdateRating(double rating)
    {
        if (rating < 1 || rating > 5)
        {
            throw new Exception("Rating not valid must be between 1 - 5 >:V");
        }
        this.Rating = Math.Round(rating, 2);
    }
    
}