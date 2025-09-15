using AidManager.API.Authentication.Domain.Model.Entities;

namespace AidManager.API.UserProfile.Interfaces.ACL;

public interface IUserAccountFacade
{
    Task<User?> GetUserById(int id);
    Task<string?> GetUserFullnameById(int id);
}