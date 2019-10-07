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
    public class OrderSenderTests : TestBase
    {

        [TestMethod]
        public async Task testOrderSendingRates()
        {
            // I could check the rate variable, wait 5 seconds and check it's incremented, but that's... weird? :p
        }

    }
}
