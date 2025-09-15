namespace AidManager.API.ManageTasks.Domain.Model.Commands;

public record UpdateRatingCommand(int ProjectId, double Rating);