namespace AidManager.API.Authentication.Domain.Model.Commands;

public record CreateUserCommand(string FirstName, string LastName, int Age, string Email, string Phone, string Password, string ProfileImg, int Role,
    string CompanyName, string CompanyEmail, string CompanyCountry, string TeamRegisterCode);