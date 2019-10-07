using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace OrderHandler
{
    public class PaymentProcessor : Common
    {
        private TextWriter writer;
        private int delay;
        private readonly IReceivableSourceBlock<Order> source;
        private readonly Action<Order> onCompletion;


        public PaymentProcessor(IReceivableSourceBlock<Order> source, TextWriter writer, Func<PaymentProcessor, Action<Order>> completionFactory = null, int delay = 2000)
        {
            this.writer = writer;
            this.delay = delay;
            this.source = source;
            this.onCompletion = completionFactory(this) ?? (o => { });
        }


        public void start()
        {
            consumeAsync();
        }


        protected async Task consumeAsync()
        {
            while (await source.OutputAvailableAsync())
            {
                Order order;
                while (source.TryReceive(out order))
                {
                    await processAsync(order);
                }
            }
        }



        public async Task processAsync(Order order)
        {
            if (order.paymentProcessed)                                                                             // if the order payment has already been processed ...
                throw new ApplicationException("Order payment already processed");                                      // throw an exception - this should never happen

            writer.WriteLine($"Payment Processor#{id}: {timestamp}: Order #{order.id}. Processing Payment.");       // build the start message

            order.paymentProcessed = await paymentIO(order);                                                        // process the order payment - in a real system this is probably going to be have an async IO component ao we use await

            string msg = order.paymentProcessed ? "Payment Processed" : "Payment Failed";                           // determine the message detail on whether the payment was successful or not
            writer.WriteLine($"Payment Processor#{id}: {timestamp}: Order #{order.id}. {msg}.");                    // build the completion message

            onCompletion(order);
        }


        private async Task<bool> paymentIO(Order order)
        {
            await Task.Delay(delay);
            return !order.shouldFail;
        }
    }
}
