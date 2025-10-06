namespace AidManager.API.UserProfile.Domain.Model.Commands;

public record PatchImageCommand(int UserId, string Image);