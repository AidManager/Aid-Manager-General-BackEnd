using AidManager.API.UserProfile.Domain.Model.Commands;
using AidManager.API.UserProfile.Interfaces.REST.Resources;

namespace AidManager.API.UserProfile.Interfaces.REST.Transform;

public static class UpdateUserCommandFromResourceAssembler
{
    public static UpdateUserCommand ToCommandFromResource(UpdateUserResource resource)
    {
       return new UpdateUserCommand(resource.FirstName, resource.LastName, resource.Age, resource.Phone, resource.ProfileImg, resource.Email, resource.Password);
    }
}