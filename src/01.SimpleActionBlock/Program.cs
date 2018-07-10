using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace _01.SimpleActionBlock
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
                throttle.Post(i);
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
