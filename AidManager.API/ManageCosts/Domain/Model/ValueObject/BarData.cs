namespace AidManager.API.ManageCosts.Domain.Model.Entities;

public class BarData
{
    public double SunAmount { get; set; }
    public double MonAmount { get; set; }
    public double TueAmount { get; set; }
    public double WedAmount { get; set; }
    public double ThuAmount { get; set; }
    public double FriAmount { get; set; }
    public double SatAmount { get; set; }

    public void updateData(double sunAmount, double monAmount, double tueAmount, double wedAmount, double thuAmount, double friAmount, double satAmount)
    {
        this.SunAmount = sunAmount;
        this.MonAmount = monAmount;
        this.TueAmount = tueAmount;
        this.WedAmount = wedAmount;
        this.ThuAmount = thuAmount;
        this.FriAmount = friAmount;
        this.SatAmount = satAmount;
    }
}
