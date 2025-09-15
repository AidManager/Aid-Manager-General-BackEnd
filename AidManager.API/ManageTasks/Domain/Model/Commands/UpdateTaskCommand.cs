namespace AidManager.API.ManageTasks.Domain.Model.Commands;

public record UpdateTaskCommand(int Id, string Title, string Description, DateOnly DueDate, string State, int AssigneeId, int ProjectId);

