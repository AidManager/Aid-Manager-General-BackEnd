using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AidManager.API.Shared.Domain.Repositories;
using AidManager.API.Shared.Infraestructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore;

namespace AidManager.API.Shared.Infraestructure.Persistence.EFC.Repositories
{
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
        
        // Detecta si el provider soporta transacciones (InMemory no)
        protected bool SupportsTransactions =>
            !string.IsNullOrWhiteSpace(Context.Database.ProviderName) &&
            !Context.Database.ProviderName.Contains("InMemory", StringComparison.OrdinalIgnoreCase);

        public virtual async Task<bool> AddAsync(TEntity entity)
        {
            if (SupportsTransactions)
            {
                await using var trans = await Context.Database.BeginTransactionAsync();
                try
                {
                    await Context.Set<TEntity>().AddAsync(entity);
                    await Context.SaveChangesAsync();
                    await trans.CommitAsync();
                    Console.WriteLine($"adding {entity} in BaseRepository");
                    return true;
                }
                catch (Exception)
                {
                    await trans.RollbackAsync();
                    Console.WriteLine($"error adding {entity} in BaseRepository");
                    return false;
                }
            }
            else
            {
                // Provider InMemory: sin transacción
                try
                {
                    await Context.Set<TEntity>().AddAsync(entity);
                    await Context.SaveChangesAsync();
                    Console.WriteLine($"adding {entity} in BaseRepository (no-tx)");
                    return true;
                }
                catch (Exception)
                {
                    Console.WriteLine($"error adding {entity} in BaseRepository (no-tx)");
                    return false;
                }
            }
        }

        public virtual async Task<TEntity?> FindByIdAsync(int id)
        {
            if (SupportsTransactions)
            {
                await using var trans = await Context.Database.BeginTransactionAsync();
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
            else
            {
                try
                {
                    var result = await Context.Set<TEntity>().FindAsync(id);
                    Console.WriteLine("finding by id in BaseRepository (no-tx)");
                    return result;
                }
                catch (Exception)
                {
                    Console.WriteLine("error finding by id in BaseRepository (no-tx)");
                    return null;
                }
            }
        }

        public virtual async Task<bool> Update(TEntity entity)
        {
            if (SupportsTransactions)
            {
                await using var trans = await Context.Database.BeginTransactionAsync();
                try
                {
                    Context.Set<TEntity>().Update(entity);
                    await Context.SaveChangesAsync();
                    await trans.CommitAsync();
                    Console.WriteLine($"updating {entity} in BaseRepository");
                    return true;
                }
                catch (Exception)
                {
                    await trans.RollbackAsync();
                    Console.WriteLine($"error updating {entity} in BaseRepository");
                    return false;
                }
            }
            else
            {
                try
                {
                    Context.Set<TEntity>().Update(entity);
                    await Context.SaveChangesAsync();
                    Console.WriteLine($"updating {entity} in BaseRepository (no-tx)");
                    return true;
                }
                catch (Exception)
                {
                    Console.WriteLine($"error updating {entity} in BaseRepository (no-tx)");
                    return false;
                }
            }
        }

        public virtual async Task<bool> Remove(TEntity entity)
        {
            if (SupportsTransactions)
            {
                await using var trans = await Context.Database.BeginTransactionAsync();
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
            else
            {
                try
                {
                    Context.Set<TEntity>().Remove(entity);
                    await Context.SaveChangesAsync();
                    Console.WriteLine("removing in BaseRepository (no-tx)");
                    return true;
                }
                catch (Exception)
                {
                    Console.WriteLine("error removing in BaseRepository (no-tx)");
                    return false;
                }
            }
        }

        public virtual async Task<IEnumerable<TEntity>?> ListAsync()
        {
            if (SupportsTransactions)
            {
                await using var trans = await Context.Database.BeginTransactionAsync();
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
            else
            {
                try
                {
                    var result = await Context.Set<TEntity>().ToListAsync();
                    Console.WriteLine("listing in BaseRepository (no-tx)");
                    return result;
                }
                catch (Exception)
                {
                    Console.WriteLine("error listing in BaseRepository (no-tx)");
                    return null;
                }
            }
        }
    }
}
