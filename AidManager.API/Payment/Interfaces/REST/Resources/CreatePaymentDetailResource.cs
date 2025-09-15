namespace AidManager.API.Payment.Interfaces.REST.Resources;

public record CreatePaymentDetailResource(int UserId, string CardHolderName, string CardNumber, string ExpirationDate, string CVV);