using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace _04.CancellingTasksInTheActionBlock
{
    class Program
    {
        private static byte[] work = new byte[100];
        static async Task Main(string[] args)
        {
            new Random().NextBytes(work);
            CancellationTokenSource cts = new CancellationTokenSource();
            
            // Define the throttle
            ActionBlock<int> throttle = new ActionBlock<int>(
                action: i=>DoStuff(i),
                dataflowBlockOptions: new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 3,
                    CancellationToken = cts.Token
                });
            
            // Create the work set.
            for (int i = 0; i < work.Length; i++)
            {
                Console.WriteLine($"{i:D3} : Posting Work Item {i}.");
                throttle.Post(i);
            }

            // indicate that there is no more work 
            throttle.Complete();

            // Stop processing after 2 seconds
            cts.CancelAfter(2000);

            // Wait for the work to complete.
            Task completionTask = throttle.Completion;
            try
            {
                await completionTask;
            }
            catch (TaskCanceledException e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine($"Completion Task status is {completionTask.Status}.");

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
