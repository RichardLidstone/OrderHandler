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

            PaymentProcessor payment = new PaymentProcessor(new StringWriter(), 10);
            await payment.process(order);

            Assert.IsTrue(order.paymentProcessed);
        }


        [TestMethod]
        public async Task CantReprocessOrders()
        {
            Order order = new Order();

            PaymentProcessor payment = new PaymentProcessor(new StringWriter(), 10);
            await payment.process(order);

            payment = new PaymentProcessor(new StringWriter());

            await Assert.ThrowsExceptionAsync<ApplicationException>(async () => await payment.process(order));
        }

        [TestMethod]
        public async Task DeliveryProcessorOutput()
        {
            StringWriter writer = new StringWriter();
            Order order = new Order();

            PaymentProcessor payment = new PaymentProcessor(writer, 10);
            await payment.process(order);

            Regex pattern = new Regex(@"Payment Processor#\d+: \d{2}:\d{2}\.\d{2} [ap]m: Order #\d+\. Processing Payment\.\r\nPayment Processor#\d+: \d{2}:\d{2}\.\d{2} [ap]m: Order #\d+\. Payment Processed\.");

            Assert.IsTrue(pattern.IsMatch(writer.ToString()), "got: " + writer.ToString());
        }

    }
}
