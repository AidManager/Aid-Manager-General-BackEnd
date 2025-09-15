namespace AidManager.API.Authentication.Domain.Model.Commands;

public record CreateCompanyCommand(string CompanyName, string Country, string Email, int UserId);