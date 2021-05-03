using System;
namespace PaymentGateway
{
    public interface IBank
    {
        /// <summary>
        ///  A merchant should be able to process a payment through the payment gateway and receive either a successful or unsuccessful response
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="expiryMonth"></param>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        /// <param name="cvv"></param>
        /// <param name="isSuccessful">true if the payment was successful, otherwise false</param>
        /// <returns>unique payment id from the bank</returns>
        public BankPaymentResult ProcessPayment(string cardNumber, DateTime expiryMonth, double amount, string currency, string cvv);
    }
}
