using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AidManager.API.Shared.Domain.Repositories;
using AidManager.API.Shared.Infraestructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore;

namespace AidManager.API.Shared.Infraestructure.Persistence.EFC.Repositories;

/*
 * BaseRepository class
 * this class is the base class for all repositories, is the abstract class
 * every repository in any bounded context should inherit from this class
 */

public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly AppDBContext Context;

    protected BaseRepository(AppDBContext context)
    {
        this.Context = context;
    }
    
    public virtual async Task<bool> AddAsync(TEntity entity)
    {
        await using (var trans = Context.Database.BeginTransaction())
        {
            try
            {
                await Context.Set<TEntity>().AddAsync(entity);
                await Context.SaveChangesAsync();
                await trans.CommitAsync();
                Console.WriteLine($"adding {entity.ToString()} in BaseRepository");
                return true;
            }
            catch (Exception)
            {
                await trans.RollbackAsync();
                Console.WriteLine($"error adding {entity.ToString()} in BaseRepository");
                return false;
            }
        }
        
        
    }
    public virtual async Task<TEntity?> FindByIdAsync(int id)
    {
        using (var trans = Context.Database.BeginTransaction())
        {
            try
            {
                var result = await Context.Set<TEntity>().FindAsync(id);
                await trans.CommitAsync();
                Console.WriteLine("finding by id in BaseRepository");
                return result;
            }
            catch (Exception)
            {
                await trans.RollbackAsync();
                Console.WriteLine("error finding by id in BaseRepository");
                return null;
            }
        }
    }
    public virtual async Task<bool> Update(TEntity entity)
    {
        using (var trans = Context.Database.BeginTransaction())
        {
            try
            {
                Context.Set<TEntity>().Update(entity);
                await Context.SaveChangesAsync();
                await trans.CommitAsync();
                Console.WriteLine($"updating {entity.ToString()} in BaseRepository");
                return true;
            }
            catch (Exception)
            {
                await trans.RollbackAsync();
                Console.WriteLine($"error updating {entity.ToString()} in BaseRepository");
                return false;
            }
        }
    }
    public virtual async Task<bool> Remove(TEntity entity)
    {
        using (var trans = Context.Database.BeginTransaction())
        {
            try
            {
                Context.Set<TEntity>().Remove(entity);
                await Context.SaveChangesAsync();
                await trans.CommitAsync();
                Console.WriteLine("removing in BaseRepository");
                return true;
            }
            catch (Exception)
            {
                await trans.RollbackAsync();
                Console.WriteLine("error removing in BaseRepository");
                return false;
            }
        }
        
    }

    public virtual async Task<IEnumerable<TEntity>?> ListAsync()
    {
        using (var trans = Context.Database.BeginTransaction())
        {
            try
            {
                var result = await Context.Set<TEntity>().ToListAsync();
                await trans.CommitAsync();
                Console.WriteLine("listing in BaseRepository");
                return result;
            }
            catch (Exception)
            {
                await trans.RollbackAsync();
                Console.WriteLine("error listing in BaseRepository");
                return null;
            }
        }
    }
}   