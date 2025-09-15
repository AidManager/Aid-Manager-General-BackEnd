using AidManager.API.ManageTasks.Domain.Model.Commands;
using AidManager.API.ManageTasks.Interfaces.REST.Resources;

namespace AidManager.API.ManageTasks.Interfaces.REST.Transform;

public class UpdateRatingCommandFromResource
{
    
    public static  UpdateRatingCommand ToCommandFromResource(int projectId, UpdateRatingProject resource) =>
        new UpdateRatingCommand(projectId, resource.Rating);    
}