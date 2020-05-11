// Написать приложение, считающее в раздельных потоках: 
//   a. факториал числа N, которое вводится с клавиатуры;
//   b. сумму целых чисел до N.

using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;

namespace Lesson5
{
    class Program
    {
        static int N;
        static void Main(string[] args)
        {
            Console.Write("Введите целое положительное число: ");
            N = int.Parse(Console.ReadLine());
            if (N == 0)
            {
                Console.WriteLine("0! = 1");
                Console.Read();
                return;
            }

            for (int i = 1; i <= 5; i++)
            {
                Thread thread = new Thread(ThreadMethodFastFactorial) { Name = i.ToString() };
                thread.Start();
                Thread thread2 = new Thread(ThreadMethodSumm) { Name = i.ToString() };
                thread2.Start();
            }
            Console.ReadLine();
        }

        private static void ThreadMethodFactorial()
        {
            BigInteger fact = 1;
            for (int i = 1; i <= N; i++)
            {
                fact *= i;
                Console.WriteLine("Поток {0}: {1}! = {2}", Thread.CurrentThread.Name, i, fact);
                Thread.Sleep(200);
            }
        }

        private static void ThreadMethodFastFactorial()
        {
            if (N == 1 || N == 2)
            {
                Console.WriteLine("Поток {0}: {1}! = {2}", Thread.CurrentThread.Name, N, N);
                Thread.Sleep(200);
                return;
            }
            bool[] u = new bool[N + 1];
            List<Tuple<int, int>> p = new List<Tuple<int, int>>();
            for (int i = 2; i <= N; ++i)
            {
                if (!u[i])
                {
                    int k = N / i;
                    int c = 0;
                    while (k > 0)
                    {
                        c += k;
                        k /= i;
                    }
                    p.Add(new Tuple<int, int>(i, c));
                    int j = 2;
                    while (i * j <= N)
                    {
                        u[i * j] = true;
                        ++j;
                    }
                }
                
            }
            
            BigInteger r = 1;
            for (int i = p.Count - 1; i >= 0; --i)
            {
                r *= BigInteger.Pow(p[i].Item1, p[i].Item2);
                if (i > 0)
                    Console.WriteLine("Поток {0}: промежуточный результат = {1}", Thread.CurrentThread.Name, r);
                else
                    Console.WriteLine("Поток {0}: {1}! = {2}", Thread.CurrentThread.Name, N, r);
                Thread.Sleep(200);
            }
        }

        private static void ThreadMethodSumm()
        {
            BigInteger summ = 0;
            for (int i = 1; i <= N; i++)
            {
                summ = summ + i;
                Console.WriteLine("Поток {0}: сумма до {1} = {2}", Thread.CurrentThread.Name, i, summ);
                Thread.Sleep(200);
            }
        }

        private static void ThreadMethodFastSumm()
        {
            BigInteger summ = N * (N + 1) / 2;
            Console.WriteLine("Поток {0}: сумма до {1} = {2}", Thread.CurrentThread.Name, N, summ);
            Thread.Sleep(200);
        }    

    }
}
