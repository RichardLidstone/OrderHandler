using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace OrderHandler
{
    public class DeliveryProcessor : Common
    {
        private TextWriter writer;
        private BufferBlock<Order> source;

        public DeliveryProcessor(BufferBlock<Order> source, TextWriter writer)
        {
            this.writer = writer;
            this.source = source;
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
                    await handleOrder(order);
                }
            }
        }


        public async Task handleOrder(Order order)
        {
            if (order.delivered)                                                                                    // if the order has already been delivered ...
                throw new ApplicationException("Order already delivered");                                              // throw an exception - this should never happen

            order.delivered = true;                                                                                 // process the order delivery - this is probably going to be have an async IO component and use await

            string msg = $"Delivery Processor#{id}: {timestamp}: Order #{order.id}. Ready for Delivery.";           // build the success message
            writer.WriteLine(msg);                                                                                  // write the message
        }
    }
}
