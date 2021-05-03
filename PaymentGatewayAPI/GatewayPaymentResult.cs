using System;
namespace PaymentGateway
{
    public class GatewayPaymentResult
    {
        public string ID { get; set; }

        // we may need a different type for a GatewayPaymentResult status to the one used by the bank
        public BankStatusEnum Status { get; set; }
    }
}
