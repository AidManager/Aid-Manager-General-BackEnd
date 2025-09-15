using AidManager.API.ManageTasks.Domain.Model.Commands;

namespace AidManager.API.ManageTasks.Application.Internal.OutboundServices;

public interface ITaskEventHandlerService
{
    Task HandleAddTeamMember(AddTeamMemberCommand command);

    Task HandleUpdateTeamMember(int userId, int projectId, int oldUserId);
    
    Task HandleRemoveTeamMember(int userId, int projectId);
    
}