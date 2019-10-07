using System;
using System.Collections.Generic;
using System.Text;

namespace OrderHandler
{
    public class Order : Common
    {

        public Order() : base()
        {
            paymentProcessed = false;
            delivered = false;
        }

        public bool paymentProcessed { get; internal set; }
        public bool delivered { get; internal set; }
    }
}
