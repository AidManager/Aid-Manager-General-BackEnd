using AidManager.API.Authentication.Domain.Model.Aggregates;
using AidManager.API.Authentication.Domain.Model.Commands;
using AidManager.API.Authentication.Domain.Repositories;
using AidManager.API.Authentication.Domain.Services;
using AidManager.API.Shared.Domain.Repositories;

namespace AidManager.API.Authentication.Application.Internal.CommandServices;

public class CompanyCommandService(ICompanyRepository companyRepository, IUnitOfWork unitOfWork) : ICompanyCommandService
{
    public async Task<bool> Handle(EditCompanyIdCommand command)
    {
        var company = await companyRepository.GetCompanyById(command.CompanyId);
        if (company == null)
        {
            throw new Exception("Company not found");
        }
        
        company.Update(command);
        await companyRepository.Update(company);
        await unitOfWork.CompleteAsync();
        
        return true;
    }

    public async Task<bool> Handle(DeleteCompanyCommand command)
    {
        var company = await companyRepository.FindCompanyByCompanyId(command.CompanyId);
        if (company == null)
        {
            throw new Exception("Company not found");
        }
        
        await companyRepository.Remove(company);
        await unitOfWork.CompleteAsync();
        return true;
    }

    public async Task<Company?> Handle(CreateCompanyCommand command)
    {
        var company = new Company(command);
        
        var result = await companyRepository.CreateCompany(company);
        await unitOfWork.CompleteAsync();
        return result;
    }

    public async Task<Company?> Handle(ValidateRegisterCode command)
    {
        
        var company = await companyRepository.FindCompanyByRegisterCode(command.TeamRegisterCode);
        if (company == null)
        {
            throw new Exception("Not Valid Register Code");
        }
        await unitOfWork.CompleteAsync();
        return company;
    }

}