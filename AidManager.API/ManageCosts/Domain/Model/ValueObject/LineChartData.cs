namespace AidManager.API.ManageCosts.Domain.Model.Entities;

public class LineChartData
{
    public double Data1 { get; set; } 
    public double Data2 { get; set; }
    public double Data3 { get; set; }
    public double Data4 { get; set; }
    public double Data5 { get; set; }
    public double Data6 { get; set; }
    public double Data7 { get; set; }

    public void UpdateData(double data1, double data2, double data3, double data4, double data5, double data6, double data7)
    {
        this.Data1 = data1;
        this.Data2 = data2;
        this.Data3 = data3;
        this.Data4 = data4;
        this.Data5 = data5;
        this.Data6 = data6;
        this.Data7 = data7;
    }
    
}