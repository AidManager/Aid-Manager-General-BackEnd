namespace AidManager.API.ManageTasks.Interfaces.REST.Resources;

public record TaskItemResource(int Id, string Title, string Description, DateOnly CreatedAt , DateOnly DueDate, string State, int AssigneeId, string AssigneeName, string AssigneeImage , int ProjectId);