using AidManager.API.Payment.Domain.Model.Aggregates;
using AidManager.API.Payment.Domain.Model.Queries;
using AidManager.API.Payment.Domain.Repositories;

namespace AidManager.API.Payment.Application.Internal.QueryServices;

public class PaymentDetailQueryService(IPaymentDetailRepository paymentDetailRepository) : IPaymentDetailQueryService
{
    public async Task<PaymentDetail> Handle(GetPaymentDetailByIdQuery query)
    {
        Console.WriteLine("query service called");
        return await paymentDetailRepository.FindByIdAsync(query.Id);
    }
}