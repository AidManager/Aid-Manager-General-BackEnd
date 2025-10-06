namespace AidManager.API.ManageTasks.Interfaces.REST.Resources;

public record UpdateTaskItemResource(string Title, string Description, DateOnly DueDate, string State, int assigneeId);