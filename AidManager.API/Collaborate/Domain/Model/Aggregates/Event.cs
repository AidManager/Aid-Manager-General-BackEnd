using AidManager.API.Collaborate.Domain.Model.Commands;

namespace AidManager.API.Collaborate.Domain.Model.Entities;

public class Event
{
    public int Id { get; private set; }
    public string Name { get; set; }
    public string Date { get; private set; }
    public string Location { get; set; }
    public string Description { get; set; }
    public string Color { get; private set; }
    public int ProjectId { get; private set; }

    public Event()
    {
        
    }
    
    public Event(string Name, string Date, string Location, string Description, string Color, int ProjectId)
    {
        this.Name = Name;
        this.Location = Location;
        this.Description = Description;
        this.Color = Color;
        this.Date = Date;
        this.ProjectId = ProjectId;
    }

    public Event(CreateEventCommand command)
    {
        this.Name = command.Name;
        this.Location = command.Location;
        this.Description = command.Description;
        this.Color = command.Color;
        this.Date = command.Date;
        this.ProjectId = command.ProjectId;
    }
    
}