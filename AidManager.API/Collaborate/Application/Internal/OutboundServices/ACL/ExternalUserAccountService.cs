using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.UserProfile.Interfaces.ACL;

namespace AidManager.API.Collaborate.Application.Internal.OutboundServices.ACL;

public class ExternalUserAccountService(IUserAccountFacade userAccountFacade)
{
    public async Task<string?> GetUserFullnameById(int id)
    {
        return await userAccountFacade.GetUserFullnameById(id);
    }
    
    public async Task<User?> GetUserById(int id)
    {
        return await userAccountFacade.GetUserById(id);
    }
    
}