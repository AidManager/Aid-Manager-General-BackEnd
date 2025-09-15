using AidManager.API.Authentication.Domain.Model.Aggregates;
using AidManager.API.Authentication.Domain.Services;

namespace AidManager.API.Authentication.Interfaces.ACL;

public interface IAuthenticationFacade
{
    Task<bool> DeleteCompany(int userId);
    Task<Company?> ExistsCompanyByEmail(string Email);
    Task<Company?> CreateCompany(string companyName, string country, string email, int userId);
    Task<Company?> ValidateRegisterCode(string TeamRegisterCode);
    Task<Company?> GetCompanyByCompanyId(int Id);
}