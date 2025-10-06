using AidManager.API.Collaborate.Domain.Model.ValueObjects;

namespace AidManager.API.Collaborate.Domain.Model.Commands;

public record CreatePostCommand(
    string Title,
    string Subject,
    string Description,
    int CompanyId,
    int UserId,
    List<string> Images
);