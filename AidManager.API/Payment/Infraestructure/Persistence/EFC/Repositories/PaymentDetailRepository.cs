using AidManager.API.Payment.Domain.Model.Aggregates;
using AidManager.API.Payment.Domain.Repositories;
using AidManager.API.Shared.Domain.Repositories;
using AidManager.API.Shared.Infraestructure.Persistence.EFC.Configuration;
using AidManager.API.Shared.Infraestructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AidManager.API.Payment.Infraestructure.Persistence.EFC.Repositories;

public class PaymentDetailRepository : BaseRepository<PaymentDetail>, IPaymentDetailRepository
{
    public PaymentDetailRepository(AppDBContext context) : base(context) {}

    // Si tu interfaz exige este método, mantenlo pero en async y con guard de transacciones
    public async Task<PaymentDetail?> CreatePaymentDetail(PaymentDetail entity)
    {
        if (SupportsTransactions)
        {
            await using var tx = await Context.Database.BeginTransactionAsync();
            try
            {
                await Context.Set<PaymentDetail>().AddAsync(entity);
                await Context.SaveChangesAsync();
                await tx.CommitAsync();
                Console.WriteLine("PaymentDetail created successfully");
                return entity;
            }
            catch
            {
                await tx.RollbackAsync();
                Console.WriteLine("Error creating Payment detail");
                return null;
            }
        }
        else
        {
            // InMemory (sin transacción)
            try
            {
                await Context.Set<PaymentDetail>().AddAsync(entity);
                await Context.SaveChangesAsync();
                Console.WriteLine("PaymentDetail created successfully (no-tx)");
                return entity;
            }
            catch
            {
                Console.WriteLine("Error creating Payment detail (no-tx)");
                return null;
            }
        }
    }

    // ⚠️ Este método en tu código original sombrea al AddAsync del base (firma distinta).
    //   Mejor haz override para preservar la misma API y evitar confusiones.
    public override Task<bool> AddAsync(PaymentDetail entity)
    {
        Console.WriteLine("adding PaymentDetail to repository");
        return base.AddAsync(entity);
    }

    // OJO: El nombre sugiere uno solo por Id; tu implementación devuelve lista.
    // Si la interfaz realmente pide IEnumerable<PaymentDetail>, lo dejo igual,
    // pero en async puro:
    public async Task<IEnumerable<PaymentDetail>> FindByIdAsync(int id)
    {
        Console.WriteLine("find by id in PaymentDetailRepository");
        return await Context
            .Set<PaymentDetail>()
            .Where(b => b.Id == id)
            .ToListAsync();
    }
}
