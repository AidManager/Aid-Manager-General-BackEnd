using AidManager.API.Payment.Domain.Model.Commands;

namespace AidManager.API.Payment.Domain.Model.Aggregates;

public class PaymentDetail
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public string CardHolderName { get; private set; }
    public string CardNumber { get; private set; }
    public string ExpirationDate { get; private set; }
    public string CVV { get; private set; }

    protected PaymentDetail()
    {
        this.CardHolderName = "nothing";
        this.CardNumber = "nothing";
        this.ExpirationDate = "nothing";
        this.CVV = "nothing";
    }

    public PaymentDetail(CreatePaymentDetailCommand command)
    {
        this.UserId = command.UserId;
        this.CardHolderName = command.CardHolderName;
        this.CardNumber = command.CardNumber;
        this.ExpirationDate = command.ExpirationDate;
        this.CVV = command.CVV;
    }
}