using AidManager.API.ManageTasks.Domain.Model.Aggregates;

namespace AidManager.API.ManageTasks.Domain.Model.Commands;

public record DeleteTaskCommand(int Id, int ProjectId);