namespace AidManager.API.Collaborate.Domain.Model.Commands;

public record AddProjectImageCommand(int ProjectId, List<string> ImagesUrl);