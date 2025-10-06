using AidManager.API.ManageTasks.Domain.Model.Commands;
using AidManager.API.ManageTasks.Interfaces.REST.Resources;

namespace AidManager.API.ManageTasks.Interfaces.REST.Transform;

public class UpdateProjectCommandFromResourceAssembler
{
    public static UpdateProjectCommand ToCommandFromResource(int projectId,UpdateProjectResource resource) =>
        new UpdateProjectCommand(projectId, resource.Name, resource.Description, resource.ImageUrl, resource.CompanyId, resource.ProjectDate.ToString(), resource.ProjectTime.ToString(), resource.ProjectLocation);
}