namespace AidManager.API.ManageTasks.Interfaces.REST.Resources;

public record CreateTaskItemResource(string Title, string Description, DateOnly DueDate, string State, int AssigneeId);