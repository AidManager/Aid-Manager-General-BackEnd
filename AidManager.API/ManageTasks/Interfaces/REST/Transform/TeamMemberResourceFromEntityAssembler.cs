using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.ManageTasks.Interfaces.REST.Resources;

namespace AidManager.API.ManageTasks.Interfaces.REST.Transform;

public static class TeamMemberResourceFromEntityAssembler
{
    public static TeamMemberResource ToResourceFromEntity(User teamMember)
    {
        var name = teamMember.FirstName + " " + teamMember.LastName; 
        return new TeamMemberResource(teamMember.Id, name, teamMember.Email, teamMember.Phone, teamMember.ProfileImg);
    }
}