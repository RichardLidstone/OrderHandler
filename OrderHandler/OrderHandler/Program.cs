using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace OrderHandler
{
    class Program
    {


        static void Main(string[] args)
        {
            int numPaymentProcessors = promptForInteger("payment processes");
            int numDeliveryProcessors = promptForInteger("delivery processes");


            BufferBlock<Order> paymentQueue = new BufferBlock<Order>();
            BufferBlock<Order> deliveryQueue = new BufferBlock<Order>();


            /*
             *  A Func that takes a payment processer as a parameter - the function then returns an order processing 
             *  closure that has access to the delivery queue and the payment processor. It determines what
             *  to do with an order after processing: A failed order is sent directly back to the payment 
             *  processor after a 5 second delay, but successful orders are queued for delivery
             */
            Func<PaymentProcessor,Action<Order>> afterPayment = paymentProcessor =>
            {
                return order =>
                {
                    if (order.paymentProcessed)                                                         // if the order has been successfully paid ...
                    {
                        deliveryQueue.Post(order);                                                          // queue the order for delivery
                    }
                    else                                                                                // if the order payment failed ...
                    {
                        order.shouldFail = false;                                                           // set the order to pass next time
                        Task.Run(async () =>                                                                // fire and forget an async Task ...
                        {
                            await Task.Delay(5000);                                                             // wait for 5 seconds
                            /*
                            paymentQueue.Post(order);                                                           // put the order back on the end of the queue, which is sub-optimal
                            /*/
                            await paymentProcessor.processAsync(order);                                         // directly call the payment processor, skipping the queue
                            //*/
                        });
                    }
                };
            };


            var sender = new OrderSender(paymentQueue, Console.Out);
            sender.start();

            for(int i=0; i<numPaymentProcessors; i++)
            {
                var processor = new PaymentProcessor(paymentQueue, Console.Out, afterPayment);
                processor.start();
            }

            for (int i = 0; i < numDeliveryProcessors; i++)
            {
                var delivery = new DeliveryProcessor(deliveryQueue, Console.Out);
                delivery.start();
            }

            deliveryQueue.Completion.Wait();                                                    // this stops the program from exiting before all orders are delivered (in theory - in practice, at the moment, ctrl+c kills it dead)
        }



        static Regex positiveInteger = new Regex(@"\d+", RegexOptions.Compiled);
        /// <summary>
        /// Prompt the user for a positive integer - will simply keep asking until it gets one
        /// </summary>
        /// <param name="question">The detail of the user prompt</param>
        /// <returns>a positibve integer</returns>
        static int promptForInteger(string question)
        {
            string num = "";

            while (!positiveInteger.IsMatch(num))
            {
                Console.Write($"Please enter the number of {question}: ");
                num = Console.ReadLine();
            }
            
            return int.Parse(num);                                                                      // we've determined the string is a positive integer - this can only fail if the user has exceeded Int32.MaxValue (that's a lot of processors)
        }

    }
}
