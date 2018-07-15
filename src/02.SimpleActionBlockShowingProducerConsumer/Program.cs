using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace _02.SimpleActionBlockShowingProducerConsumer
{
    class Program
    {
        private static byte[] work = new byte[100];
        static async Task Main(string[] args)
        {
            new Random().NextBytes(work);

            // Define the throttle
            ActionBlock<int> throttle = new ActionBlock<int>(i=>DoStuff(i));
            
            // Create the work set.
            for (int i = 0; i < work.Length; i++)
            {
                Console.WriteLine($"{i:D3} : Posting Work Item {i}.");
                throttle.Post(i);
                Thread.Sleep(50);
            }

            // indicate that there is no more work 
            throttle.Complete();

            // Wait for the work to complete.
            await throttle.Completion;

            Console.WriteLine("All done!");
            Console.ReadLine();
        }

        static void DoStuff(int data)
        {
            int wait = work[data];
            Console.WriteLine($"{data:D3} : Work will take {wait}ms");
            Thread.Sleep(wait);
        }
    }
}
