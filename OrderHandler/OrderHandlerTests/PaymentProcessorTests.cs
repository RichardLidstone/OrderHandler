using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrderHandler;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OrderHandlerTests
{

    [TestClass]
    public class PaymentProcessorTests : TestBase
    {
        [TestMethod]
        public async Task processPayment()
        {
            Order order = new Order();

            PaymentProcessor payment = new PaymentProcessor(null, new StringWriter(), delay: 10);
            await payment.processAsync(order);

            Assert.IsTrue(order.paymentProcessed);
        }


        [TestMethod]
        public async Task CantReprocessOrders()
        {
            Order order = new Order();

            PaymentProcessor payment = new PaymentProcessor(null, new StringWriter(), delay: 10);
            await payment.processAsync(order);

            payment = new PaymentProcessor(null, new StringWriter(), delay: 10);

            await Assert.ThrowsExceptionAsync<ApplicationException>(async () => await payment.processAsync(order));
        }


        [TestMethod]
        public async Task DeliveryProcessorOutput()
        {
            StringWriter writer = new StringWriter();
            Order order = new Order();

            PaymentProcessor payment = new PaymentProcessor(null, writer, delay: 10);
            await payment.processAsync(order);

            Regex pattern = new Regex(@"Payment Processor#\d+: \d{2}:\d{2}\.\d{2} [ap]m: Order #\d+\. Processing Payment\.\r\nPayment Processor#\d+: \d{2}:\d{2}\.\d{2} [ap]m: Order #\d+\. Payment Processed\.");

            Assert.IsTrue(pattern.IsMatch(writer.ToString()), "got: " + writer.ToString());
        }

    }
}
