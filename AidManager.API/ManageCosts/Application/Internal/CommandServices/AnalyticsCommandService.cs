using AidManager.API.ManageCosts.Domain.Model.Aggregates;
using AidManager.API.ManageCosts.Domain.Model.Commands;
using AidManager.API.ManageCosts.Domain.Model.Entities;
using AidManager.API.ManageCosts.Domain.Repositories;
using AidManager.API.ManageCosts.Domain.Services;
using AidManager.API.Shared.Domain.Repositories;

namespace AidManager.API.ManageCosts.Application.Internal.CommandServices;

public class AnalyticsCommandService(
    IAnalyticsRepository analyticsRepository,
    IUnitOfWork unitOfWork
    ): IAnalyticsCommandService
{

    public async Task<Analytics?> Handle(CreateAnalyticsCommand command)
    {
        try
        {
            var analytic = new Analytics(command);
            await analyticsRepository.AddAsync(analytic);
            
            await unitOfWork.CompleteAsync();
            
            return analytic;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<Analytics?> Handle(UpdateBarDataPaymentsCommand command)
    {
        try
        {
            
            var analytic = await analyticsRepository.GetAnalyticsByProjectId(command.Id);
            
            if (analytic == null)
            {
                return null;
            }
            
            analytic.BarData = command.BarData;
            await analyticsRepository.Update(analytic);
            await unitOfWork.CompleteAsync();
            
            return analytic;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<Analytics?> Handle(UpdateAnalyticProgressbarCommand command)
    {
        try
        {
            
            var analytic = await analyticsRepository.GetAnalyticsByProjectId(command.Id);
            
            if (analytic == null)
            {
                return null;
            }
            
            analytic.Progressbar = command.Progressbar; 
            await analyticsRepository.Update(analytic);
            await unitOfWork.CompleteAsync();
            return analytic;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<Analytics?> Handle(UpdateAnalyticStatusCommand command)
    {
        try
        {

            var analytic = await analyticsRepository.GetAnalyticsByProjectId(command.Id);
            
            if (analytic == null)
            {
                return null;
            }
            
            analytic.Status = command.Status;
            await analyticsRepository.Update(analytic);

            await unitOfWork.CompleteAsync();
            
            return analytic;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<Analytics?> Handle(UpdateLinesChartBarCommand command)
    {
        try
        {

            var analytic = await analyticsRepository.GetAnalyticsByProjectId(command.Id);
            
            if (analytic == null)
            {
                return null;
            }

            var linesChartBarData = new List<LineChartData>();
            linesChartBarData.AddRange(command.Lines);
            analytic.LinesChartBarData = linesChartBarData;
            await analyticsRepository.Update(analytic);

            await unitOfWork.CompleteAsync();
            
            return analytic;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<Analytics?> Handle(UpdateAnalyticTasksCommand command)
    {
        try
        {

            var analytic = await analyticsRepository.GetAnalyticsByProjectId(command.Id);
            
            if (analytic == null)
            {
                return null;
            }
            
            analytic.Tasks = command.Tasks;
            await analyticsRepository.Update(analytic);

            await unitOfWork.CompleteAsync();
            
            return analytic;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
}