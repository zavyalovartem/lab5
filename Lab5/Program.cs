using System;
using System.IO;

namespace Lab5
{
    class Program
    {
        static void Main(string[] args)
        {
            SOLE();
            //Exp();
        }

        private static void SOLE()
        {
            Console.WriteLine("Введите размеры матрицы коэффициентов через запятую");
            var nLeft = Console.ReadLine().Split(',');
            var nLeftInt = int.Parse(nLeft[0]);
            var mLeftInt = int.Parse(nLeft[1]);
            Console.WriteLine("Введите размер вектора");
            var mRight = int.Parse(Console.ReadLine());
            double[,] left = new double[nLeftInt, mLeftInt];
            double[,] right = new double[1, mRight];
            for (var i = 0; i < mLeftInt; i++)
            {
                Console.WriteLine("Введите коэффициенты через запятую");
                var leftStr = Console.ReadLine().Split(',');
                for (var j = 0; j < nLeftInt; j++)
                {
                    left[i, j] = int.Parse(leftStr[j]);
                }
            }
            Console.WriteLine("Введите коэффициенты вектора через запятую");
            var rightStr = Console.ReadLine().Split(',');
            for (var i = 0; i < mRight; i++)
            {
                right[0, i] = int.Parse(rightStr[i]);
            }
            var rightCoeffs = new Matrix(right);
            var leftCoeffs = new Matrix(left);
            var sole = new SOLEInvertibleMatrix();
            var res = sole.Solve(leftCoeffs, rightCoeffs);
            Console.WriteLine(" ");
            Console.WriteLine("Вектор решения:");
            foreach (var coeff in res.data)
            {
                Console.WriteLine(Math.Round(coeff));
            }
        }

        private static void Exp()
        {
            var sw = new StreamWriter("exp.txt");
            for (var i = 2; i <= 10; i++)
            {
                var matrix = new Matrix(i, i);
                var rnd = new Random();
                while (true)
                {
                     res = matrix.CreateInvertibleMatrix();
                     if (res != null)
                     {
                        break;
                     }
                    else
                    {
                        var x = rnd.Next() % (i);
                        var y = rnd.Next() % (i);
                        matrix[x, y] += 1;
                        matrix.amountOfOperations = 0;
                    }
                }
                Console.WriteLine(i + " " + matrix.amountOfOperations);
            }
        }
    }
}
