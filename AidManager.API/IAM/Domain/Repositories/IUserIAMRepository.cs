using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.IAM.Domain.Model.Aggregates;
using AidManager.API.Shared.Domain.Repositories;

namespace AidManager.API.IAM.Domain.Repositories;

public interface IUserIAMRepository: IBaseRepository<UserAuth>
{
    Task<UserAuth?> FindByUsernameAsync(string username);
    bool ExistsByUsername(string username);
}