using AidManager.API.Authentication.Domain.Model.Aggregates;
using AidManager.API.Authentication.Domain.Model.Commands;
using AidManager.API.Authentication.Domain.Model.Queries;
using AidManager.API.Authentication.Domain.Services;

namespace AidManager.API.Authentication.Interfaces.ACL.Services;

public class AuthenticationFacade(ICompanyCommandService commandService, ICompanyQueryService queryService) : IAuthenticationFacade
{
    public async Task<Company?> CreateCompany(string companyName, string country, string email, int userId)
    {
        var createCompanyCommand = new CreateCompanyCommand(companyName, country, email, userId);
        var company = await commandService.Handle(createCompanyCommand);
        return company;
    }

    public async Task<Company?> ValidateRegisterCode(string TeamRegisterCode)
    {
        var validateRegisterCodeCommand = new ValidateRegisterCode(TeamRegisterCode);
        return await commandService.Handle(validateRegisterCodeCommand);
    }

    public async Task<Company?> GetCompanyByCompanyId(int companyId)
    {
        var getCompanyByUserId = new GetCompanyByIdQuery(companyId);
        return await queryService.Handle(getCompanyByUserId);
    }
    
    public async Task<Company?> ExistsCompanyByEmail(string email)
    {
        var existsCompanyByEmailQuery = new GetCompanyByEmail(email);
        return await queryService.Handle(existsCompanyByEmailQuery);
    }
    
  
    
    public async Task<bool> DeleteCompany(int userId)
    {
        var deleteCompanyCommand = new DeleteCompanyCommand(userId);
        return await commandService.Handle(deleteCompanyCommand);
    }
    
}