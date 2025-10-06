using AidManager.API.Payment.Domain.Model.Aggregates;
using AidManager.API.Payment.Domain.Model.Queries;

namespace AidManager.API.Payment.Application.Internal.QueryServices;

public interface IPaymentDetailQueryService
{
    Task<PaymentDetail> Handle(GetPaymentDetailByIdQuery query);
}