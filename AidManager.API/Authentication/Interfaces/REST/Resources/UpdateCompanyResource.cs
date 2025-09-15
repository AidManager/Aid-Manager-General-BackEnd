namespace AidManager.API.Authentication.Interfaces.REST.Resources;

public record UpdateCompanyResource(
    string CompanyName, string Country, string Email
    );