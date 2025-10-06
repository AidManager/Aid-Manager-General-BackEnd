namespace AidManager.API.ManageTasks.Domain.Model.Commands;

public record AddTeamMemberCommand(int UserId, int ProjectId);