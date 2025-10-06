using AidManager.API.Payment.Domain.Model.Aggregates;
using AidManager.API.Payment.Domain.Model.Commands;
using AidManager.API.Payment.Domain.Repositories;
using AidManager.API.Payment.Domain.Services;
using AidManager.API.Shared.Domain.Repositories;

namespace AidManager.API.Payment.Application.Internal.CommandServices;

public class PaymentDetailCommandService(IPaymentDetailRepository paymentDetailRepository, IUnitOfWork unitOfWork) : IPaymentDetailCommandService
{
    public async Task<PaymentDetail> Handle(CreatePaymentDetailCommand command)
    {
        var paymentDetail = new PaymentDetail(command);
        var result = await paymentDetailRepository.CreatePaymentDetail(paymentDetail);
        await unitOfWork.CompleteAsync();
        
        return result;
    }

}