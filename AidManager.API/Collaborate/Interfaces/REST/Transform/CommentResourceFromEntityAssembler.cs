using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.Collaborate.Domain.Model.ValueObjects;
using AidManager.API.Collaborate.Interfaces.REST.Resources;

namespace AidManager.API.Collaborate.Interfaces.REST.Transform;

public static class CommentResourceFromEntityAssembler
{
    public static CommentResource ToResourceFromEntity(Comments? comments, User user)
    {
        return new CommentResource(
            comments.Id,
            comments.Comment,
            comments.UserId,
            user.FirstName + " " + user.LastName,
            user.Email,
            user.ProfileImg,
            comments.PostId,
            comments.TimeOfComment.ToString("yyyy-MM-dd HH:mm:ss")
        );
    }

}