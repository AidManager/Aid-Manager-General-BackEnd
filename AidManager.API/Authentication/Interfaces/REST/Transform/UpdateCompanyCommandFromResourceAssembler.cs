using AidManager.API.Authentication.Domain.Model.Commands;
using AidManager.API.Authentication.Interfaces.REST.Resources;

namespace AidManager.API.Authentication.Interfaces.REST.Transform;

public class UpdateCompanyCommandFromResourceAssembler
{
    public static EditCompanyIdCommand ToCommandFromResource(UpdateCompanyResource resource, int companyId)
    {
        return new EditCompanyIdCommand(
            resource.CompanyName,
            resource.Country,
            resource.Email,
            companyId
        );
    }
}