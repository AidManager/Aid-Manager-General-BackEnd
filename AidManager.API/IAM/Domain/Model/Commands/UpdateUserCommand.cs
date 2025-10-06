namespace AidManager.API.IAM.Domain.Model.Commands;

public record UpdateUserCommand(string OldUsername,string Username, string Password, int UserRole);
