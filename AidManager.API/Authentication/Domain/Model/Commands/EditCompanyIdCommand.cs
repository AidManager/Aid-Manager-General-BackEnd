namespace AidManager.API.Authentication.Domain.Model.Commands;

public record EditCompanyIdCommand(string BrandName, string Country, string Email, int CompanyId);