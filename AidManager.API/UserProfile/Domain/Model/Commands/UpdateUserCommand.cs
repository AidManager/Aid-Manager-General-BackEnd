namespace AidManager.API.UserProfile.Domain.Model.Commands;

public record UpdateUserCommand(string FirstName, string LastName, int Age, string Phone, string ProfileImg, string Email, string Password);