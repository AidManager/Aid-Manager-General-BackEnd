namespace AidManager.API.Authentication.Interfaces.REST.Resources;

public record CreateUserResource(string FirstName, string LastName, int Age, string Email, string Phone, string Password, string ProfileImg, int Role,
    string CompanyName, string CompanyEmail, string CompanyCountry, string TeamRegisterCode);