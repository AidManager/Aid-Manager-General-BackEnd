using AidManager.API.Authentication.Domain.Model.Aggregates;
using AidManager.API.Authentication.Interfaces.REST.Resources;

namespace AidManager.API.Authentication.Interfaces.REST.Transform;

public static class CreateCompanyResourceFromEntityAssembler
{
    public static GetCompanyResource ToResourceFromEntity(Company company)
    {
        return new GetCompanyResource(company.Id, company.CompanyName, company.Country, company.Email, company.ManagerId, company.TeamRegisterCode);
    }
}