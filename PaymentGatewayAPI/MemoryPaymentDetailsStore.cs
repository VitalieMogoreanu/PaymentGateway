using System;
using System.Collections.Generic;

namespace PaymentGateway
{
    public class MemoryPaymentDetailsStore : IPaymentDetailsStore
    {
        private Dictionary<string, PaymentDetails> _paymentDetails = new Dictionary<string, PaymentDetails>();

        public PaymentDetails RetrievePaymentDetails(string id)
        {
            return _paymentDetails[id];
        }

        public void SavePaymentDetails(PaymentDetails paymentDetails)
        {
            _paymentDetails.Add(paymentDetails.ID, paymentDetails);
        }
    }
}
