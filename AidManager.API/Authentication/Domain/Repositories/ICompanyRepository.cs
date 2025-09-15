using AidManager.API.Authentication.Domain.Model.Aggregates;
using AidManager.API.Shared.Domain.Repositories;

namespace AidManager.API.Authentication.Domain.Repositories;

public interface ICompanyRepository : IBaseRepository<Company>
{
    Task<Company?> CreateCompany(Company company);
    Task<Company?> FindCompanyByCompanyId(int companyId);
    
    Task<Company?> GetCompanyById(int companyId);
    
    Task<Company?> GetCompanyByEmail(string email);
    Task<bool> ExistsByID(int id);
    
    Task<Company?> FindCompanyByRegisterCode(string registerCode);
    
}