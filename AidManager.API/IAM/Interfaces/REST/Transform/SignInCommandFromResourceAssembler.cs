using AidManager.API.IAM.Domain.Model.Commands;
using AidManager.API.IAM.Interfaces.REST.Resources;

namespace AidManager.API.IAM.Interfaces.REST.Transform;

public static class SignInCommandFromResourceAssembler
{
    public static SignInCommand ToCommandFromResource(SignInResource resource)
    {
        return new SignInCommand(resource.Email, resource.Password);
    }
}