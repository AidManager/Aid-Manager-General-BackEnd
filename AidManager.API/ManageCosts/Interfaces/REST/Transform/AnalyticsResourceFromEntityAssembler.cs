using AidManager.API.ManageCosts.Domain.Model.Aggregates;
using AidManager.API.ManageCosts.Interfaces.REST.Resources;

namespace AidManager.API.ManageCosts.Interfaces.REST.Transform;

public class AnalyticsResourceFromEntityAssembler
{
    public static AnalyticsResource ToResourceFromEntity(Analytics entity)
    {
        return new AnalyticsResource(
            entity.Id,
            entity.ProjectId,
            entity.LinesChartBarData,
            entity.BarData,
            entity.Progressbar,
            entity.Status,
            entity.Tasks
        );
    }
}