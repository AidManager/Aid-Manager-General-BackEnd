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
    public Task<PaymentDetail> CreatePaymentDetail(PaymentDetail entity)
    {
        using (var transaction = Context.Database.BeginTransaction())
        {
            try
            {
                Context.Set<PaymentDetail>().Add(entity);
                Context.SaveChanges();
                transaction.Commit();
                Console.WriteLine("PaymentDetail created successfully");
                return Task.FromResult(entity);
            }
            catch(Exception)
            {
                Console.WriteLine("Error creating Payment detail");
                transaction.Rollback();
            }

            return Task.FromResult<PaymentDetail>(null);
        }
    }

    public Task AddAsync(PaymentDetail entity)
    {
        Console.WriteLine("adding PaymentDetail to repository");
        return base.AddAsync(entity);
    }
    
    public async Task<IEnumerable<PaymentDetail>> FindByIdAsync(int id)
    {
        Console.WriteLine("find by id in PaymentDetailRepository");
        return await Context.Set<PaymentDetail>().Where(b => b.Id == id).ToListAsync();
    }
}