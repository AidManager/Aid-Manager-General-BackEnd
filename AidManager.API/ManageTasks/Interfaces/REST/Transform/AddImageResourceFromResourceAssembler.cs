using AidManager.API.Collaborate.Domain.Model.Commands;
using AidManager.API.ManageTasks.Interfaces.REST.Resources;

namespace AidManager.API.ManageTasks.Interfaces.REST.Transform;

public class AddImageResourceFromResourceAssembler
{
    public static AddProjectImageCommand ToCommandFromResource(AddImageResource resource, int projectId) =>
        new AddProjectImageCommand(projectId, resource.ImageUrl);
}