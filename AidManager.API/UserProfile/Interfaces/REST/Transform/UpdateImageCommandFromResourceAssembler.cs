using AidManager.API.IAM.Domain.Model.Commands;
using AidManager.API.UserProfile.Domain.Model.Commands;
using AidManager.API.UserProfile.Interfaces.REST.Resources;

namespace AidManager.API.UserProfile.Interfaces.REST.Transform;

public static class UpdateImageCommandFromResourceAssembler
{
    public static PatchImageCommand ToCommandFromResource(UpdateUserImageResource resource, int userId)
    {
        return new PatchImageCommand(userId, resource.Image);
    }
}