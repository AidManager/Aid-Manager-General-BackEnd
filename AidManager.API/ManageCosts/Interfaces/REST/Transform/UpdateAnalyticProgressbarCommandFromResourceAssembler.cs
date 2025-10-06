using AidManager.API.ManageCosts.Domain.Model.Commands;
using AidManager.API.ManageCosts.Interfaces.REST.Resources;

namespace AidManager.API.ManageCosts.Interfaces.REST.Transform;

public class UpdateAnalyticProgressbarCommandFromResourceAssembler
{
    public static UpdateAnalyticProgressbarCommand ToCommandFromResource(int projectId, UpdateAnalyticProgressbarResource resource)
    {
        return new UpdateAnalyticProgressbarCommand(
            projectId,
            resource.Progressbar
        );
    }
}