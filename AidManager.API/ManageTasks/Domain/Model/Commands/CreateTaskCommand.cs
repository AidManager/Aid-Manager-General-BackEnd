namespace AidManager.API.ManageTasks.Domain.Model.Commands;

public record CreateTaskCommand(string Title, string Description, DateOnly DueDate, int ProjectId, string State, int AssigneeId);