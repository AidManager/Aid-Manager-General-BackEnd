using AidManager.API.Payment.Domain.Model.Aggregates;
using AidManager.API.Shared.Domain.Repositories;

namespace AidManager.API.Payment.Domain.Repositories;

public interface IPaymentDetailRepository: IBaseRepository<PaymentDetail>
{
    public Task<PaymentDetail> CreatePaymentDetail(PaymentDetail entity);
}