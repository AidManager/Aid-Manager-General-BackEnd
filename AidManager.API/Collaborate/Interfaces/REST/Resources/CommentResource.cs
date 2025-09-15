namespace AidManager.API.Collaborate.Interfaces.REST.Resources;

public record CommentResource(
    int Id,
    string Comment,
    int UserId,
    string UserName,
    string UserEmail,
    string UserImage,
    int PostId,
    string CommentTime
);