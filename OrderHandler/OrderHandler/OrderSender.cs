using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace OrderHandler
{
    public class OrderSender : Common
    {
        private TextWriter writer;
        private int ratePerSecond = 0;
        private int millisBetweenOrders = 0;
        private bool running = false;

        ITargetBlock<Order> orders;

        public OrderSender(ITargetBlock<Order> target, TextWriter writer)
        {
            this.writer = writer;
            orders = target;
        }


        public void start()
        {
            running = true;
            rateUpdater();                                                  // we don't want to await this call - this is unusual and provokes a VS warning
            createOrders();
        }


        public void stop()
        {
            running = false;
        }


        
        private async Task rateUpdater()
        {
            while (running)
            {
                ratePerSecond++;
                millisBetweenOrders = 1000 / ratePerSecond;
                await Task.Delay(5000);
            }
        }


        /// <summary>
        /// generate orders at given rates
        /// </summary>
        private async Task createOrders()
        {
            while (running)
            {
                var order = new Order();
                orders.Post(order);

                writer.WriteLine($"Order Sender : {timestamp}: Order #{order.id}. Sent.");

                await Task.Delay(millisBetweenOrders);
            }
            orders.Complete();
        }
    }
}
