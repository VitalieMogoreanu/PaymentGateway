using System;
using Xunit;
using PaymentGateway;
using PaymentGateway.Controllers;

namespace PaymentGatewayTests
{
    public class PaymentGatewayControllerTest
    {
        public class TestMockBank : IBank
        {
            public BankPaymentResult ExpectedResult { get; set; }

            public BankPaymentResult ProcessPayment(string cardNumber, DateTime expiryMonth, double amount, string currency, string cvv)
            {
                return this.ExpectedResult;
            }
        }

        public class TestPaymentDetailsStore : IPaymentDetailsStore
        {
            private PaymentDetails _paymentDetails;
            private string _idPassed;

            public string IDPassed { get { return _idPassed; } }

            public void SavePaymentDetails(PaymentDetails paymentDetails)
            {
                _paymentDetails = paymentDetails;
            }

            public PaymentDetails RetrievePaymentDetails(string id)
            {
                _idPassed = id;
                return _paymentDetails;
            }
        }

        [Fact]
        public void ProcessPaymentPassStatusAndIDFromBank()
        {
            var bank = new TestMockBank();
            var store = new TestPaymentDetailsStore();
            var pg = new PaymentGatewayController(bank, store, null);

            var expectedResult = new BankPaymentResult() {
                ID = Guid.NewGuid().ToString(),
                Status = BankStatusEnum.Failure,
            };

            bank.ExpectedResult = expectedResult;
            var gatewayResult = pg.ProcessPayment("12345", new DateTime(2025, 12, 01), 1, "GBP", "001");
            Assert.Equal(expectedResult.ID, gatewayResult.ID);
            Assert.Equal(expectedResult.Status, gatewayResult.Status);
        }


        [Fact]
        public void ProcessPaymentStoresPaymentDetails()
        {
            var bank = new TestMockBank();
            var store = new TestPaymentDetailsStore();
            var pg = new PaymentGatewayController(bank, store, null);

            var expectedResult = new BankPaymentResult()
            {
                ID = Guid.NewGuid().ToString(),
                Status = BankStatusEnum.Success,
            };

            var expectedPaymentDetails = new PaymentDetails
            {
                MaskedCardNumber = "*2345",
                ExpiryMonth = new DateTime(2027, 04, 01),
                Amount = 123,
                Currency = "USD",
                Cvv = "667",
            };

            bank.ExpectedResult = expectedResult;

            var gatewayResult = pg.ProcessPayment("12345",
                                    expectedPaymentDetails.ExpiryMonth,
                                    expectedPaymentDetails.Amount,
                                    expectedPaymentDetails.Currency,
                                    expectedPaymentDetails.Cvv);

            var storedDetails = store.RetrievePaymentDetails("ignored");

            Assert.Equal(expectedPaymentDetails.MaskedCardNumber, storedDetails.MaskedCardNumber);
            Assert.Equal(expectedPaymentDetails.ExpiryMonth, storedDetails.ExpiryMonth);
            Assert.Equal(expectedPaymentDetails.Amount, storedDetails.Amount);
            Assert.Equal(expectedPaymentDetails.Currency, storedDetails.Currency);
            Assert.Equal(expectedPaymentDetails.Cvv, storedDetails.Cvv);
        }

        [Fact]
        public void GetPaymentDetailsPassID()
        {
            var bank = new TestMockBank();
            var store = new TestPaymentDetailsStore();
            var pg = new PaymentGatewayController(bank, store, null);

            var expectedResult = new BankPaymentResult()
            {
                ID = Guid.NewGuid().ToString(),
                Status = BankStatusEnum.Failure,
            };

            bank.ExpectedResult = expectedResult;
            var gatewayResult = pg.ProcessPayment("12345", new DateTime(2025, 12, 01), 1, "GBP", "001");
            Assert.Equal(expectedResult.ID, gatewayResult.ID);
            Assert.Equal(expectedResult.Status, gatewayResult.Status);

            string id = Guid.NewGuid().ToString();
            var storeDetails = pg.GetPaymentDetails(id);

            Assert.Equal(id, store.IDPassed);
        }
    }
}
