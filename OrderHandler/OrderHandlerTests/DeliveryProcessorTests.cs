using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrderHandler;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text.RegularExpressions;

namespace OrderHandlerTests
{

    [TestClass]
    public class DeliveryProcessorTests : TestBase
    {

        [TestMethod]
        public void DeliveryProcessorOutput()
        {
            StringWriter writer = new StringWriter();
            Order order = new Order();

            DeliveryProcessor deliveryProcessor = new DeliveryProcessor(writer);
            deliveryProcessor.handleOrder(order);

            Regex pattern = new Regex(@"Delivery Processor#\d+: \d{2}:\d{2}\.\d{2} [ap]m: Order #\d+\. Ready for Delivery\.");
            Assert.IsTrue(pattern.IsMatch(writer.ToString()), "got: " + writer.ToString());
        }


        [TestMethod]
        public void DeliveryProcessorDoesDelivery()
        {
            Order order = new Order();

            DeliveryProcessor deliveryProcessor = new DeliveryProcessor(new StringWriter());
            deliveryProcessor.handleOrder(order);

            Assert.IsTrue(order.delivered);
        }


        [TestMethod]
        public void CantReprocessOrders()
        {
            Order order = new Order();

            DeliveryProcessor deliveryProcessor = new DeliveryProcessor(new StringWriter());
            deliveryProcessor.handleOrder(order);

            deliveryProcessor = new DeliveryProcessor(new StringWriter());

            Assert.ThrowsException<ApplicationException>(() => deliveryProcessor.handleOrder(order));
        }
    }
}
