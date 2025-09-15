using AidManager.API.Payment.Domain.Model.Aggregates;
using AidManager.API.Payment.Interfaces.REST.Resources;

namespace AidManager.API.Payment.Interfaces.REST.Transform;

public static class PaymentDetailResourceFromEntityAssembler
{
    public static PaymentDetailResource ToResourceFromEntity(PaymentDetail entity) =>
    new PaymentDetailResource(entity.Id, entity.UserId, entity.CardHolderName, entity.CardNumber, entity.ExpirationDate, entity.CVV);
}