using AidManager.API.Authentication.Domain.Model.Aggregates;
using AidManager.API.Authentication.Domain.Repositories;
using AidManager.API.Shared.Domain.Repositories;
using AidManager.API.Shared.Infraestructure.Persistence.EFC.Configuration;
using AidManager.API.Shared.Infraestructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AidManager.API.Authentication.Infrastructure.Persistence.EFC.Repositories;

public class CompanyRepository(AppDBContext context) : BaseRepository<Company>(context), ICompanyRepository
{
    public async Task<Company?> CreateCompany(Company company)
    {
            await AddAsync(company);
            return company;
    }
    
    public async Task<Company?> FindCompanyByCompanyId(int companyId)
    {
        return await Context.Set<Company>().Where(c => c.Id == companyId).FirstOrDefaultAsync();
    }

    public async Task<Company?> GetCompanyByEmail(string email)
    {
        return await Context.Set<Company>().Where(c => c.Email == email).FirstOrDefaultAsync();
    }

    public async Task<Company?> FindCompanyByRegisterCode(string registercode)
    {
        return await Context.Set<Company>().Where(c => c.TeamRegisterCode == registercode).FirstOrDefaultAsync();
    }
    
    public async Task<Company?> GetCompanyById(int id)
    {
        return await Context.Set<Company>().Where(c => c.Id == id).FirstOrDefaultAsync();
    }
    
    public async Task<bool> ExistsByID(int id)
    {
        return await Context.Set<Company>().AnyAsync(c => c.Id == id);
    }
}