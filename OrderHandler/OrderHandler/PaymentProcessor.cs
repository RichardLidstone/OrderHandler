using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OrderHandler
{
    public class PaymentProcessor : Common
    {

        private TextWriter writer;

        public PaymentProcessor(TextWriter writer)
        {
            this.writer = writer;
        }
    }
}
