using AidManager.API.Authentication.Domain.Model.Commands;
using AidManager.API.Authentication.Interfaces.REST.Resources;

namespace AidManager.API.Authentication.Interfaces.REST.Transform;

public static class CreateUserCommandFromResourceAssembler
{
    public static CreateUserCommand ToCommandFromResource(CreateUserResource resource)
    {
        return new CreateUserCommand(resource.FirstName, resource.LastName, resource.Age, resource.Email, resource.Phone, resource.Password, resource.ProfileImg, resource.Role, resource.CompanyName, resource.CompanyEmail, resource.CompanyCountry, resource.TeamRegisterCode);
    }
}