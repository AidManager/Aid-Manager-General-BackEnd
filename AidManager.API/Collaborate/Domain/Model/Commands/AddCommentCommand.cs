namespace AidManager.API.Collaborate.Domain.Model.Commands;

public record AddCommentCommand(int UserId, string Comment, int PostId);