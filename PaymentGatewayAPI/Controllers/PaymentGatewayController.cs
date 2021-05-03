using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PaymentGateway.Controllers
{
    [Route("[controller]")]
    public class PaymentGatewayController : Controller
    {
        private readonly IBank _bank;
        private readonly ILogger<PaymentGatewayController> _logger;
        private readonly IPaymentDetailsStore _store;

        public PaymentGatewayController(IBank bank, IPaymentDetailsStore store, ILogger<PaymentGatewayController> logger)
        {
            _bank = bank;
            _store = store;
            _logger = logger;
        }

        /// <summary>
        /// Masking all except last 4 digits
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns>Masked card number</returns>
        private string MaskCardNumber(string cardNumber)
        {
            if (cardNumber == null) throw new ArgumentNullException("cardNumber");
            if (cardNumber.Length < 4) throw new ArgumentOutOfRangeException("cardNumber");

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            for (int i = 0; i < cardNumber.Length; i++)
            {
                if (i < cardNumber.Length - 4)
                {
                    sb.Append('*');
                }
                else
                {
                    sb.Append(cardNumber[i]);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Store card information and forward payment requests and accepting payment responses to and from the acquiring bank
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="expiryMonth"></param>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        /// <param name="cvv"></param>
        /// <returns></returns>
        [HttpGet]
        public GatewayPaymentResult ProcessPayment(string cardNumber, DateTime expiryMonth, double amount, string currency, string cvv)
        {
            BankPaymentResult bankRes = _bank.ProcessPayment(cardNumber, expiryMonth, amount, currency, cvv);

            GatewayPaymentResult res = new GatewayPaymentResult { ID = bankRes.ID, Status = bankRes.Status };

            _store.SavePaymentDetails(
                new PaymentDetails
                {
                    ID = bankRes.ID,
                    Status = bankRes.Status,
                    // assuming we do not need to store the actual card number
                    MaskedCardNumber = MaskCardNumber(cardNumber),
                    ExpiryMonth = expiryMonth,
                    Amount = amount,
                    Currency = currency,
                    Cvv = cvv

                });
            return res;
        }

        /// <summary>
        /// Allows a merchant to retrieve details of a previously made payment
        /// Doing this will help the merchant with their reconciliation and reporting needs
        /// <param name="id">Unique payment identifier</param>
        /// <returns>The response includes a masked card number and card details along with a status code which indicates the result of the payment</returns>
        [HttpGet("{id}")]
        public PaymentDetails GetPaymentDetails(string id)
        {
            return _store.RetrievePaymentDetails(id);
        }
    }
}
