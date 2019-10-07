using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace OrderHandler
{
    public class PaymentProcessor : Common
    {
        private TextWriter writer;
        private int delay;


        public PaymentProcessor(TextWriter writer, int delay = 2000)
        {
            this.writer = writer;
            this.delay = delay;
        }


        public async Task process(Order order)
        {
            if (order.paymentProcessed)                                                                             // if the order payment has already been processed ...
                throw new ApplicationException("Order payment already processed");                                      // throw an exception - this should never happen

            writer.WriteLine($"Payment Processor#{id}: {timestamp}: Order #{order.id}. Processing Payment.");       // build the start message

            order.paymentProcessed = await paymentIO(order);                                                        // process the order payment - in a real system this is probably going to be have an async IO component ao we use await

            string msg = order.paymentProcessed ? "Payment Processed" : "Payment Failed";                           // determine the message detail on whether the payment was successful or not
            writer.WriteLine($"Payment Processor#{id}: {timestamp}: Order #{order.id}. {msg}.");                    // build the completion message
        }


        private async Task<bool> paymentIO(Order order)
        {
            await Task.Delay(delay);
            return order.shouldFail;
        }
    }
}
