using System;
using System.Collections.Generic;
using System.Text;

namespace OrderHandler
{
    public class Order : Common
    {
        public Order() : base() => delivered = false;

        public bool delivered { get; internal set; }
    }
}
