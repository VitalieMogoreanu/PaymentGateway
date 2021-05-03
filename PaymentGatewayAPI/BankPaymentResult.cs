using System;
namespace PaymentGateway
{
    public class BankPaymentResult
    {
        public string ID { get; set; }

        public BankStatusEnum Status { get; set; }
    }

    public enum BankStatusEnum { Success, Failure }
}
