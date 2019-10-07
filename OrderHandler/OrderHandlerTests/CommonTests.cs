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


        [TestMethod]
        public void ObjectsSetIdsWhenInitialised()
        {
            var order = new Order();
            Assert.AreEqual(1, order.id);

            var dp = new DeliveryProcessor(null);
            Assert.AreEqual(1, dp.id);

            order = new Order();
            Assert.AreEqual(2, order.id);
        }


        /// <summary>
        /// Ensures Ids can't be set from outside the class
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
