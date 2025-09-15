using AidManager.API.Authentication.Domain.Model.Aggregates;
using AidManager.API.Authentication.Domain.Model.Queries;

namespace AidManager.API.Authentication.Domain.Services;

public interface ICompanyQueryService
{
    Task<Company?> Handle(GetCompanyByIdQuery query);
    
    Task<Company?> Handle(GetCompanyByEmail query);
    
}