using AidManager.API.Authentication.Domain.Model.Aggregates;
using AidManager.API.Authentication.Interfaces.ACL;
using AidManager.API.IAM.Interfaces.ACL;

namespace AidManager.API.UserManagement.UserProfile.Application.Internal.OutboundServices.ACL;

public class ExternalUserAuthService(IIamContextFacade iamContextFacade, IAuthenticationFacade authenticationFacade)
{
    
    public async Task<Company?> GetCompanyByEmail(string email)
    {
        var companyFound = await authenticationFacade.ExistsCompanyByEmail(email);
        if (companyFound != null)
        {
            return companyFound;
        }
        return null;
    }
    public async Task CreateUsername(string username, string password, int role)
    {
        try
        {
            await iamContextFacade.CreateUser(username, password, role);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task DeleteUser (string username)
    {
        await iamContextFacade.DeleteUser(username);
    }
    
    public async Task UpdateUser(string oldUsername,string username, string password, int role)
    {
        await iamContextFacade.UpdateUserData(oldUsername,username, password, role);
    }
    
    public async Task<int> FetchUserIdByUsername(string username)
    {
        return await iamContextFacade.FetchUserIdByUsername(username);
    }
    
    public async Task<string> FetchUsernameByUserId(int userId)
    {
        return await iamContextFacade.FetchUsernameByUserId(userId);
    }
    
    public async Task<Company> AuthenticateCode(string registerCode)
    {
        var company = await authenticationFacade.ValidateRegisterCode(registerCode);
        if (company == null)
        {
            throw new Exception("AUTH ERROR: Register Code NOT Valid");
        }

        return company;
    }
    
    public async Task<Company> FetchCompanyByCompanyId(int comapnyId)
    {
        var company = await authenticationFacade.GetCompanyByCompanyId(comapnyId);
        if (company == null)
        {
            Console.WriteLine(comapnyId);
            throw new Exception("ERROR: Company not by Company ID" );
        }

        return company;
    }
    
    public async Task<Company?> CreateCompany(string companyName, string country, string email, int userId)
    {
        try
        {
            var company = await authenticationFacade.CreateCompany(companyName, country, email, userId);
            return company;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<bool> DeleteCompany(int userId)
    {
        try
        {
            return await authenticationFacade.DeleteCompany(userId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    
    
    
}