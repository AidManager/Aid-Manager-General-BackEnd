namespace AidManager.API.Payment.Interfaces.REST.Resources;

public record PaymentDetailResource(int Id, int UserId, string CardHolderName, string CardNumber, string ExpirationDate, string CVV);