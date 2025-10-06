using AidManager.API.Payment.Domain.Model.Commands;
using AidManager.API.Payment.Interfaces.REST.Resources;

namespace AidManager.API.Payment.Interfaces.REST.Transform;

public static class CreatePaymentDetailCommandFromResourceAssembler
{
    public static CreatePaymentDetailCommand ToCommandFromResource(CreatePaymentDetailResource resource) =>
        new CreatePaymentDetailCommand(resource.UserId, resource.CardHolderName, resource.CardNumber, resource.ExpirationDate, resource.CVV);
}