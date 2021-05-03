using System;
namespace PaymentGateway
{
    public interface IPaymentDetailsStore
    {
        public void SavePaymentDetails(PaymentDetails paymentDetails);

        public PaymentDetails RetrievePaymentDetails(string id);
    }
}
