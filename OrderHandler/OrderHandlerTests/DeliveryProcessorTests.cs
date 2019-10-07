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
    public class DeliveryProcessorTests : TestBase
    {

        [TestMethod]
        public async Task DeliveryProcessorOutput()
        {
            StringWriter writer = new StringWriter();
            Order order = new Order();

            DeliveryProcessor deliveryProcessor = new DeliveryProcessor(writer);
            await deliveryProcessor.handleOrder(order);

            Regex pattern = new Regex(@"Delivery Processor#\d+: \d{2}:\d{2}\.\d{2} [ap]m: Order #\d+\. Ready for Delivery\.");
            Assert.IsTrue(pattern.IsMatch(writer.ToString()), "got: " + writer.ToString());
        }


        [TestMethod]
        public async Task DeliveryProcessorDoesDelivery()
        {
            Order order = new Order();

            DeliveryProcessor deliveryProcessor = new DeliveryProcessor(new StringWriter());
            await deliveryProcessor.handleOrder(order);

            Assert.IsTrue(order.delivered);
        }


        [TestMethod]
        public async Task CantReprocessOrders()
        {
            Order order = new Order();

            DeliveryProcessor deliveryProcessor = new DeliveryProcessor(new StringWriter());
            await deliveryProcessor.handleOrder(order);

            deliveryProcessor = new DeliveryProcessor(new StringWriter());

            await Assert.ThrowsExceptionAsync<ApplicationException>(async () =>  await deliveryProcessor.handleOrder(order));
        }
    }
}
