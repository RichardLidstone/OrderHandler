using System;
using System.Collections.Concurrent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrderHandler;

namespace OrderHandlerTests
{
    
    public class TestBase
    {

        [TestInitialize]
        public void testInit()
        {
            Common.nextId = new ConcurrentDictionary<Type, int>();
        }
    }
}
