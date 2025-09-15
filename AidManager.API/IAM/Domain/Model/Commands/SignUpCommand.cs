namespace AidManager.API.IAM.Domain.Model.Commands;

public record SignUpCommand(string Username, string Password, int UserRole);