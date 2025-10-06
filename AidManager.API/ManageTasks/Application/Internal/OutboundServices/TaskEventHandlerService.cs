using AidManager.API.ManageTasks.Application.Internal.OutboundServices.ACL;
using AidManager.API.ManageTasks.Domain.Model.Aggregates;
using AidManager.API.ManageTasks.Domain.Model.Commands;
using AidManager.API.ManageTasks.Domain.Repositories;
using AidManager.API.Shared.Domain.Repositories;

namespace AidManager.API.ManageTasks.Application.Internal.OutboundServices;

public class TaskEventHandlerService(ITeamMemberRepository teamMemberRepository, ExternalUserService externalUserService, IUnitOfWork unitOfWork) : ITaskEventHandlerService
{
    public async Task HandleAddTeamMember(AddTeamMemberCommand command)
    {
        if (!await teamMemberRepository.Exists(command.UserId, command.ProjectId))
        {
            await teamMemberRepository.AddAsync(new ProjectTeamMembers(command.UserId, command.ProjectId));
            await unitOfWork.CompleteAsync();
        }
    }

    public async Task HandleUpdateTeamMember(int userId, int projectId, int olduserId)
    {
        await teamMemberRepository.Remove(new ProjectTeamMembers(olduserId, projectId));
        if (!await teamMemberRepository.Exists(userId, projectId))
        {
            await teamMemberRepository.AddAsync(new ProjectTeamMembers(userId, projectId));
        }
        await unitOfWork.CompleteAsync();

        
    }

    public async Task HandleRemoveTeamMember(int userId, int projectId)
    {
        await teamMemberRepository.Remove(new ProjectTeamMembers(userId, projectId));
        await unitOfWork.CompleteAsync();
    }
}