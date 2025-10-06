using AidManager.API.ManageCosts.Domain.Model.Commands;
using AidManager.API.ManageCosts.Interfaces.REST.Resources;

namespace AidManager.API.ManageCosts.Interfaces.REST.Transform;

public class UpdateAnalyticStatusCommandFromResourceAssembler
{
    public static UpdateAnalyticStatusCommand ToCommandFromResource(int projectId, UpdateAnalyticStatusResource resource)
    {
        return new UpdateAnalyticStatusCommand(
            projectId,
            resource.Status
        );
    }
}