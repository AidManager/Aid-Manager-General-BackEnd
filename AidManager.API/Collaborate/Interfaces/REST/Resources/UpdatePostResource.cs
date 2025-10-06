namespace AidManager.API.Collaborate.Interfaces.REST.Resources;

public record UpdatePostResource(string Title,
    string Subject,
    string Description,
    List<string> Images
   );