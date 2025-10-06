namespace AidManager.API.Collaborate.Interfaces.REST.Resources;

public record CreatePostResource(string Title, string Subject, string Description, int CompanyId, int UserId, List<string> Images);

