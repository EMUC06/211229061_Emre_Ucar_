using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _211229061_Emre_Ucar
{
    internal static class Program
    {
        static ArrayList numbersList = new ArrayList();
        static ArrayList evenNumbersList = new ArrayList();
        static ArrayList oddNumbersList = new ArrayList();
        static ArrayList primeNumbersList = new ArrayList();

        static Random random = new Random();
      
        [STAThread]
        static void Main()
        {
            for (int i = 1; i <= 1000000; i++)
            {
                numbersList.Add(i);
            }

            int segmentSize = numbersList.Count / 4;

            Thread thread1 = new Thread(() => FindPrimeNumbers(1, 1, segmentSize));
            Thread thread2 = new Thread(() => FindPrimeNumbers(2, segmentSize + 1, 2 * segmentSize));
            Thread thread3 = new Thread(() => FindEvenNumbers(3, 2 * segmentSize + 1, 3 * segmentSize));
            Thread thread4 = new Thread(() => FindOddNumbers(4, 3 * segmentSize + 1, numbersList.Count));

            thread1.Start();
            thread2.Start();
            thread3.Start();
            thread4.Start();

            thread1.Join();
            thread2.Join();
            thread3.Join();
            thread4.Join();

            Console.WriteLine("Even Numbers: " + string.Join(", ", evenNumbersList.ToArray()));
            Console.WriteLine("Odd Numbers: " + string.Join(", ", oddNumbersList.ToArray()));
            Console.WriteLine("Prime Numbers: " + string.Join(", ", primeNumbersList.ToArray()));

            Console.ReadLine();

        }
        static void FindEvenNumbers(int threadIndex, int start, int end)
        {
            

            for (int i = start; i <= end; i++)
            {
                int number = (int)numbersList[i];

                if (number % 2 == 0)
                {
                    lock (evenNumbersList)
                    {
                        evenNumbersList.Add(number);
                    }
                }
            }

        }

        static void FindOddNumbers(int threadIndex, int start, int end)
        {
          
            end = Math.Min(end, numbersList.Count - 1);

            for (int i = start; i <= end; i++)
            {
                int number = (int)numbersList[i];

                if (number % 2 != 0)
                {
                    lock (oddNumbersList)
                    {
                        oddNumbersList.Add(number);
                    }
                }
            }

        }

        static void FindPrimeNumbers(int threadIndex, int start, int end)
        {
            for (int i = start; i <= end; i++)
            {
                int number = (int)numbersList[i];

                if (IsPrime(number))
                {
                    lock (primeNumbersList)
                    {
                        primeNumbersList.Add(number);
                    }
                }
            }
        }

        static bool IsPrime(int num)
        {
            if (num < 2)
                return false;

            for (int i = 2; i <= Math.Sqrt(num); i++)
            {
                if (num % i == 0)
                    return false;
            }
            return true;
        }
    }
}
