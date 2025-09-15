using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.Authentication.Domain.Repositories;
using AidManager.API.Shared.Infraestructure.Persistence.EFC.Configuration;
using AidManager.API.Shared.Infraestructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AidManager.API.Authentication.Infrastructure.Persistence.EFC.Repositories;

public class UserRepository(AppDBContext context) : BaseRepository<User>(context), IUserRepository
{
    public async Task<List<User>> FindUsersByCompanyId(int companyId)
    {
        return await Context.Set<User>().Where(p => p.CompanyId == companyId).ToListAsync();

    }

    public async Task<User?> FindUserByEmail(string email)
    {
        
        return await Context.Set<User>().FirstOrDefaultAsync(x => x.Email == email);
        
    }

    public async Task<User?> FindUserById(int id)
    {
        return await Context.Set<User>().FirstOrDefaultAsync(x => x.Id == id);
    }
}