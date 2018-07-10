using System;
using System.Threading;
using System.Threading.Tasks;

namespace _00.BasicSerialImplementation
{
    class Program
    {
        private static byte[] work = new byte[100];
        static void Main(string[] args)
        {
            new Random().NextBytes(work);

            for (int i = 0; i < work.Length; i++)
            {
                DoStuff(i);
            }

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
