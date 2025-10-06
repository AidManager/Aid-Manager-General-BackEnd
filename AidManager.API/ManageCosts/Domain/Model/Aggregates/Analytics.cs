
using AidManager.API.ManageCosts.Domain.Model.Commands;
using AidManager.API.ManageCosts.Domain.Model.Entities;

namespace AidManager.API.ManageCosts.Domain.Model.Aggregates;

public class Analytics
{
    public int Id { get; set; }
    
    public int ProjectId { get; set; }
    
    public List<LineChartData> LinesChartBarData { get; set; }
    
    public List<BarData> BarData { get; set; }
    
    public List<double> Tasks { get; set; }
    
    public List<double> Progressbar { get; set; }
    
    public List<double> Status { get; set; }
    

    public Analytics()
    {
        LinesChartBarData = new List<LineChartData>();
        BarData = new List<BarData>();
        Tasks = new List<double>();
        Progressbar = new List<double>();
        Status = new List<double>();
        
    }
    
    public Analytics(int id, int projectId, List<LineChartData> linesChartBarData, List<BarData> barData, List<double> progressbar, List<double> status, List<double> tasks)
    {
        this.ProjectId = projectId;
        this.LinesChartBarData = linesChartBarData;
        this.BarData = barData;
        this.Progressbar = progressbar;
        this.Status = status;
        this.Tasks = tasks;
    }
    
    public Analytics(CreateAnalyticsCommand command)
    {
        this.ProjectId = command.ProjectId;
        this.LinesChartBarData = command.linesChartBarData;
        this.BarData = command.barData;
        this.Progressbar = command.progressbar;
        this.Status = command.status;
        this.Tasks = command.tasks;
    }
}