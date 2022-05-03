using System;
using System.Threading;

namespace ProducerConsumerSynchronized
{
    class Program
    {
        static readonly object lock1 = new object();
        static bool[] arr = new bool[3];
        static void Main(string[] args)
        {
            Thread t1 = new Thread(Producer);
            Thread t2 = new Thread(Consumer);
            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();
        }
        static void Producer()
        {
            while(true)
            {
                if (Monitor.TryEnter(lock1))
                {
                    try
                    {
                        while (true)
                        {
                            if (arr[2] == true)
                            {
                                Console.WriteLine("Producer fik ikke lov til at producere: 3");
                                break;
                            }
                            else if (arr[0] == false)
                            {
                                arr[0] = true;
                                Console.WriteLine("Producer fik lov til at producere: 1");
                            }
                            else if (arr[1] == false)
                            {
                                arr[1] = true;
                                Console.WriteLine("Producer fik lov til at producere: 2");
                            }
                            else
                            {
                                arr[2] = true;
                                Console.WriteLine("Producer fik lov til at producere: 3");
                                break;
                            }
                        }
                    }
                    finally
                    {
                        Monitor.PulseAll(lock1);
                        Monitor.Wait(lock1);
                    }
                }
            }
        }
        static void Consumer()
        {
            while (true) {
                if (Monitor.TryEnter(lock1))
                {
                    try
                    {
                        while (true)
                        {
                            if (arr[0] == false)
                            {
                                Console.WriteLine("Consumer fik ikke lov til at tage: 0");
                                break;
                            }
                            else if (arr[2] == true)
                            {
                                arr[2] = false;
                                Console.WriteLine("Consumer fik lov til at tage: 2");

                            }
                            else if (arr[1] == true)
                            {
                                arr[1] = false;
                                Console.WriteLine("Consumer fik lov til at tage: 1");
                            }
                            else
                            {
                                arr[0] = false;
                                Console.WriteLine("Consumer fik lov til at tage: 0");
                                break;
                            }
                        }
                    }
                    finally
                    {
                        Monitor.PulseAll(lock1);
                        Monitor.Wait(lock1);
                    }
                }
            }
        }
    }
}
