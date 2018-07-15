using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace _03.MaxDegreesOfParallelismAsAThrottle
{
    class Program
    {
        private static int tasksInProgress = 0;
        private static byte[] work = new byte[100];
        static async Task Main(string[] args)
        {
            new Random().NextBytes(work);
            // Define the throttle
            ActionBlock<int> throttle = new ActionBlock<int>(
                action: i=>DoStuff(i),
                dataflowBlockOptions: new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 3
                });
            
            // Create the work set.
            for (int i = 0; i < work.Length; i++)
            {
                Console.WriteLine($"{i:D3} : Posting Work Item {i}.");
                throttle.Post(i);
                Thread.Sleep(25);
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
            tasksInProgress++;
            int wait = work[data];
            Console.WriteLine($"{data:D3} : [TIP:{tasksInProgress}] Work will take {wait}ms");
            Thread.Sleep(wait);
            tasksInProgress--;
        }
    }
}
