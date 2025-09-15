using AidManager.API.Authentication.Interfaces.REST.Resources;
using AidManager.API.Collaborate.Domain.Model.ValueObjects;

namespace AidManager.API.Collaborate.Interfaces.REST.Resources;

public record PostResource(int Id, string Title,
    string Subject,
    string Description,
    DateTime PostTime,
    int CompanyId,
    int UserId,
    string UserName,    
    string UserImage,
    string Email,
    int Rating,
    List<string> Images,
    List<CommentResource> CommentsList);