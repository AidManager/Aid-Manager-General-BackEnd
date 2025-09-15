using AidManager.API.Authentication.Domain.Model.Aggregates;
using AidManager.API.Authentication.Domain.Model.Commands;

namespace AidManager.API.Authentication.Domain.Services;

public interface ICompanyCommandService
{
    Task<bool> Handle(EditCompanyIdCommand command);
    Task<bool> Handle(DeleteCompanyCommand command);
    Task<Company?> Handle(CreateCompanyCommand command);
    
    Task<Company?> Handle(ValidateRegisterCode command);
    
}