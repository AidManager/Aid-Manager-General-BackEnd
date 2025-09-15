using AidManager.API.ManageCosts.Domain.Model.Aggregates;
using AidManager.API.ManageCosts.Domain.Repositories;
using AidManager.API.Shared.Infraestructure.Persistence.EFC.Configuration;
using AidManager.API.Shared.Infraestructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AidManager.API.ManageCosts.Infraestructure.Repositories;

public class AnalyticsRepository(AppDBContext context): BaseRepository<Analytics>(context), IAnalyticsRepository
{
    
    
     
    public async Task<Analytics?> GetAnalyticsById(int id)
    {
            try
            {
                return await Context.Set<Analytics>().FindAsync(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        
    }
    
    public async Task<Analytics?> GetAnalyticsByProjectId(int projectId)
    {
        try
        {
            return await Context.Set<Analytics>()
                .AsNoTracking()
                .Include(x => x.BarData)
                .Include(x => x.LinesChartBarData)
                .AsSplitQuery() // Configure QuerySplittingBehavior
                .FirstOrDefaultAsync(x => x.ProjectId == projectId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    public async Task<Analytics?> FindAnalyticsById(int id)
    {
        
            try
            {
                return await Context.Set<Analytics>().FindAsync(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        
    }
    
}