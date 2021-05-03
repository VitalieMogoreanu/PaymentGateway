using System;
namespace PaymentGateway
{
    public class PaymentDetails
    {
        public string ID { get; set; }

        public BankStatusEnum Status { get; set; }

        public string MaskedCardNumber { get; set; }

        public DateTime ExpiryMonth { get; set; }

        public double Amount { get; set; }

        public string Currency { get; set; }

        public string Cvv { get; set; }
    }
}
