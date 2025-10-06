using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.Authentication.Domain.Repositories;
using AidManager.API.IAM.Domain.Model.Aggregates;
using AidManager.API.IAM.Domain.Repositories;
using AidManager.API.Shared.Infraestructure.Persistence.EFC.Configuration;
using AidManager.API.Shared.Infraestructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AidManager.API.IAM.Infrastructure.Persistence.EFC.Repositories;

public class UserIAMRepository(AppDBContext context): BaseRepository<UserAuth>(context), IUserIAMRepository
{
    public async Task<UserAuth?> FindByUsernameAsync(string username)
    {
        return await Context.Set<UserAuth>().FirstOrDefaultAsync(user => user.Username.Equals(username));
    }

    public bool ExistsByUsername(string username)
    {
        return Context.Set<UserAuth>().Any(user => user.Username.Equals(username));
    }
}