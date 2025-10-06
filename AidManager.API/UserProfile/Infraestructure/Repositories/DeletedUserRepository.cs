using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.Authentication.Domain.Repositories;
using AidManager.API.Shared.Infraestructure.Persistence.EFC.Configuration;
using AidManager.API.Shared.Infraestructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AidManager.API.Authentication.Infrastructure.Persistence.EFC.Repositories;

public class DeletedUserRepository (AppDBContext context) : BaseRepository<DeletedUser>(context), IDeletedUserRepository
{ 
    public async Task<List<DeletedUser>> FindUsersByCompanyId(int companyId)
    {
        return await Context.Set<DeletedUser>().Where(p => p.CompanyId == companyId).ToListAsync();

    }

    public async Task<DeletedUser?> FindUserByEmail(string email)
    {
        
        return await Context.Set<DeletedUser>().FirstOrDefaultAsync(x => x.Email == email);
        
    }

    public async Task<DeletedUser?> FindUserById(int id)
    {
        return await Context.Set<DeletedUser>().FirstOrDefaultAsync(x => x.Id == id);
    }
}