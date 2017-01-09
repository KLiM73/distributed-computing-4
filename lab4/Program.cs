using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using MPI;

namespace lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var mppi = new MPI.Environment(ref args))
            {

                if (Communicator.world.Rank == 0)
                {
                    Random r = new Random();
                    double[,] mas = new double[5, 5];
                    for (int i = 0; i < 5; i++)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            mas[i, j] = r.Next(0, 10);
                        }
                    }

                    Console.WriteLine("Исходный Массив:");
                    for (int i = 0; i < mas.GetLength(0); i++)
                    {
                        for (int j = 0; j < mas.GetLength(1); j++)
                        {
                            Console.Write(mas[i, j] + " ");
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                    double temp;
                    for (int repeat = 0; repeat < 5; repeat++)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            if (mas[i, 0] > mas[i + 1, 0])
                            {
                                for (int j = 0; j < 5; j++)
                                {
                                     temp = mas[i, j];
                                    mas[i, j] = mas[i + 1, j];
                                    mas[i + 1, j] = temp;
                                }
                            }
                        }
                    }

                    Communicator.world.Send(mas, 1, 0);
                }

                if (Communicator.world.Rank == 1)
                {
                    double[,] msg = Communicator.world.Receive<double[,]>(Communicator.world.Rank - 1, 0);
                    Console.WriteLine("Итоговый Массив:");
                    for (int i = 0; i < msg.GetLength(0); i++)
                    {
                        for (int j = 0; j < msg.GetLength(1); j++)
                        {
                            Console.Write(msg[i, j] + " ");
                        }
                        Console.WriteLine();
                    }
                }
            }
        }
    }
}