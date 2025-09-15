namespace AidManager.API.Collaborate.Domain.Model.Commands;

public record UpdatePostCommand(int Id, string Title, string Subject, string Description, int CompanyId, int UserId, List<string> Images);