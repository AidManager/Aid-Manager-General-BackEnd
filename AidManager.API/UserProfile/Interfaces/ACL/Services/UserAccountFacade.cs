using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.Authentication.Domain.Model.Queries;
using AidManager.API.Authentication.Domain.Services;

namespace AidManager.API.UserProfile.Interfaces.ACL.Services;

public class UserAccountFacade(IUserCommandService commandService, IUserQueryService queryService) : IUserAccountFacade
{
    public async Task<User?> GetUserById(int id)
    {
        var findUserByIdQuery = new GetUserByIdQuery(id);
        return await queryService.FindUserById(findUserByIdQuery);
    }

    public async Task<string?> GetUserFullnameById(int id)
    {
        var findUserByIdQuery = new GetUserByIdQuery(id);
        var user = await queryService.FindUserById(findUserByIdQuery);
        return user?.FirstName + " " + user?.LastName;
    }
}