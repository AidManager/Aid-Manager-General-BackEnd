using AidManager.API.ManageCosts.Domain.Model.Commands;
using AidManager.API.ManageCosts.Domain.Model.Entities;
using AidManager.API.ManageCosts.Interfaces.REST.Resources;

namespace AidManager.API.ManageCosts.Interfaces.REST.Transform;

public class UpdateBarDataCommandFromResourceAssembler
{
    public static UpdateBarDataPaymentsCommand ToCommandFromResource(int projectId, UpdateBarDataResource resource)
    {
        var barDataList = resource.BarData.Select(b => new BarData
        {
            FriAmount = b.friAmount,
            MonAmount = b.monAmount,
            SatAmount = b.satAmount,
            SunAmount = b.sunAmount,
            ThuAmount = b.thuAmount,
            TueAmount = b.tueAmount,
            WedAmount = b.wedAmount
        }).ToList();
        
        return new UpdateBarDataPaymentsCommand(
            projectId,
            barDataList
        );
        
    }
}