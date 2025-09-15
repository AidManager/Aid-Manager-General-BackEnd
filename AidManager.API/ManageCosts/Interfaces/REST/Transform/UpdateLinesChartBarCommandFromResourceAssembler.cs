using AidManager.API.ManageCosts.Domain.Model.Commands;
using AidManager.API.ManageCosts.Domain.Model.Entities;
using AidManager.API.ManageCosts.Interfaces.REST.Resources;

namespace AidManager.API.ManageCosts.Interfaces.REST.Transform;

public class UpdateLinesChartBarCommandFromResourceAssembler
{
    public static UpdateLinesChartBarCommand ToCommandFromResource(int projectId, UpdateLinesChartBarResource resource)
    {
        
        var linesChartBarList = resource.Lines.Select(l => new LineChartData
        {
            Data1 = l.data1,
            Data2 = l.data2,
            Data3 = l.data3,
            Data4 = l.data4,
            Data5 = l.data5,
            Data6 = l.data6,
            Data7 = l.data7
        }).ToList();
        
        
        return new UpdateLinesChartBarCommand(
            projectId,
            linesChartBarList
        );
    }
}