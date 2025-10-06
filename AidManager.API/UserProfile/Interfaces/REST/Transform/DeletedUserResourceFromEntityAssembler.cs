using AidManager.API.Authentication.Domain.Model.Aggregates;
using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.UserProfile.Interfaces.REST.Resources;

namespace AidManager.API.UserProfile.Interfaces.REST.Transform;

public class DeletedUserResourceFromEntityAssembler
{
    
    public static DeletedUserResource ToResource(DeletedUser user, Company company)
    {
        return new DeletedUserResource(user.Id, user.FirstName +" "+ user.LastName, user.Age, user.Email, user.Phone, user.Password, user.ProfileImg, user.Role, user.CompanyId, company.CompanyName, company.Email, company.Country, user.DeletedAt);
    }
    
}