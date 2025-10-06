using AidManager.API.Authentication.Domain.Model.Aggregates;
using AidManager.API.Authentication.Domain.Model.Queries;
using AidManager.API.Authentication.Domain.Repositories;
using AidManager.API.Authentication.Domain.Services;
using AidManager.API.Authentication.Infrastructure.Persistence.EFC.Repositories;
using AidManager.API.Shared.Domain.Repositories;

namespace AidManager.API.Authentication.Application.Internal.QueryServices;

public class CompanyQueryService(ICompanyRepository companyRepository, IUnitOfWork unitOfWork) : ICompanyQueryService
{
    public async Task<Company?> Handle(GetCompanyByIdQuery query)
    {
        return await companyRepository.FindCompanyByCompanyId(query.Id);
    }

    public async Task<Company?> Handle(GetCompanyById query)
    {
        return await companyRepository.GetCompanyById(query.CompanyId);
    }
    

    public async Task<Company?> Handle(GetCompanyByEmail query)
    {
        return await companyRepository.GetCompanyByEmail(query.Email);
    }
}