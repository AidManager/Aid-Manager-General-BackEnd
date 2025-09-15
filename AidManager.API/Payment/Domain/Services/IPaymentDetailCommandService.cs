using AidManager.API.Payment.Domain.Model.Aggregates;
using AidManager.API.Payment.Domain.Model.Commands;

namespace AidManager.API.Payment.Domain.Services;

public interface IPaymentDetailCommandService
{
    Task<PaymentDetail> Handle(CreatePaymentDetailCommand command);
}