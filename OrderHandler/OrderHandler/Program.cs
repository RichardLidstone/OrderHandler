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


            Func<PaymentProcessor,Action<Order>> afterPayment = paymentProcessor =>
            {
                return order =>
                {
                    if (order.paymentProcessed)
                    {
                        deliveryQueue.Post(order);
                    }
                    else
                    {
                        order.shouldFail = false;
                        Task.Run(async () =>
                        {
                            await Task.Delay(5000);
                            /*
                            paymentQueue.Post(order);
                            /*/
                            await paymentProcessor.processAsync(order);
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

            var delivery = new DeliveryProcessor(deliveryQueue, Console.Out);

            delivery.start();

            paymentQueue.Completion.Wait();
        }


        static Regex positiveInteger = new Regex(@"\d+", RegexOptions.Compiled);
        static int promptForInteger(string question)
        {
            string num = "";

            while (!positiveInteger.IsMatch(num))
            {
                Console.Write($"Please enter the number of {question}: ");
                num = Console.ReadLine();
            }

            return int.Parse(num);
        }

    }
}
