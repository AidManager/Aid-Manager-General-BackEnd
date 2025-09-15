namespace AidManager.API.Collaborate.Domain.Model.Commands;

public record RemoveFavoritePostCommand(int PostId, int UserId);