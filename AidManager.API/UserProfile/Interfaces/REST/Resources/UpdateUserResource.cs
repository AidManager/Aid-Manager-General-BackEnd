namespace AidManager.API.UserProfile.Interfaces.REST.Resources;

public record UpdateUserResource(string FirstName, string LastName, int Age, string Phone, string ProfileImg, string Email, string Password);