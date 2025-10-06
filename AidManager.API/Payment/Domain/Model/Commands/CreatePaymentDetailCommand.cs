namespace AidManager.API.Payment.Domain.Model.Commands;

public record CreatePaymentDetailCommand(int UserId, string CardHolderName, string CardNumber, string ExpirationDate, string CVV);