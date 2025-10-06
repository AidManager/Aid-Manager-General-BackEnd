namespace AidManager.API.Authentication.Interfaces.REST.Resources;

public record GetCompanyResource(int Id, string CompanyName, string Country, string Email, int ManagerId, string TeamRegisterCode);