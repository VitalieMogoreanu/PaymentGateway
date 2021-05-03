using System;
namespace PaymentGateway
{
    public class MockBank : IBank
    {
        public BankPaymentResult ProcessPayment(string cardNumber, DateTime expiryMonth, double amount, string currency, string cvv)
        {
            return new BankPaymentResult {
                ID = Guid.NewGuid().ToString(),
                Status = BankStatusEnum.Success
            };
        }
    }
}
