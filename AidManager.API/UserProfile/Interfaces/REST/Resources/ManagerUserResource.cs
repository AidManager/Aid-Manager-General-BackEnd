namespace AidManager.API.UserProfile.Interfaces.REST.Resources;

public record ManagerUserResource(int Id, string Name, int Age, string Email, string Phone, string Password, string ProfileImg, string Role, int CompanyId ,string CompanyName, string CompanyEmail, string CompanyCountry, string TeamRegisterCode);
