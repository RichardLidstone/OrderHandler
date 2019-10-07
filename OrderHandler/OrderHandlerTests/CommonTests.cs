using Microsoft.CSharp.RuntimeBinder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrderHandler;
using System.Collections.Concurrent;
using System;

namespace OrderHandlerTests
{
    [TestClass]
    public class CommonTests : TestBase
    {

        /// <summary>
        /// Tests Ids are correctly allocated sequentially to Types.
        /// Has multiple asserts which is generally considered bad form - I've still 
        /// done it because this is all about testing the encapsulation of Ids to types 
        /// as different objects are instantiated. I think that's OK?
        /// </summary>
        [TestMethod]
        public void ObjectsSetIdsWhenInitialised()
        {
            var order = new Order();
            Assert.AreEqual(1, order.id, "first Order");

            var dp = new DeliveryProcessor(null, null);
            Assert.AreEqual(1, dp.id, "first DeliveryProcessor");

            var pp = new PaymentProcessor(null, null);
            Assert.AreEqual(1, pp.id, "first PaymentProcessor");

            order = new Order();
            Assert.AreEqual(2, order.id, "second Order");
        }


        /// <summary>
        /// Tests that Ids can't be changed
        /// </summary>
        [TestMethod]
        public void ObjectsIdsAreImmutable()
        {
            dynamic order = new Order();                                        // grab an order as dynamic to avoid compile time type checking

            Assert.ThrowsException<RuntimeBinderException>(() =>                // we expect a RuntimeBinderException when we ...
            {
                order.id = 42;                                                      // try to set the order Id
            });
        }
    }
}
