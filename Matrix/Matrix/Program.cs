/* Даны две двумерных матрицы размерностью 100х100 каждая. 
 * Задача: написать приложение, производящее их параллельное умножение. 
 * Матрицы заполняются случайными целыми числами от 0 до 10.
*/

using System;
using System.Threading.Tasks;
using Array2DLib;

namespace Matrix
{
    class Program
    {
        static void Main(string[] args)
        {
            Array2D m1 = new Array2D(100, 100, 0, 10); // ("..//..//..//..//matrix1.txt");
            Console.WriteLine("Матрица А: ");
            m1.Print();

            Array2D m2 = new Array2D(100, 100, 0, 10); // ("..//..//..//..//matrix2.txt");
            Console.WriteLine("Матрица B: ");
            m2.Print();

            Console.WriteLine("Запуск умножения матриц.");
            Array2D m3 = new Array2D(m1.RowCount, m2.ColumnCount);
            Task<int>[] tasks = new Task<int>[m1.RowCount * m2.ColumnCount];
            for (int t = 0, i = 0; i < m1.RowCount; i++)
            {
                for (int j = 0; j < m2.ColumnCount; j++, t++)
                {
                    tasks[t] = Task.Factory.StartNew(() => TaskMethodMulti(m1.GetRow(i), m2.GetColumn(j)));
                    m3.SetCell(i, j, tasks[t].Result);
                }
            }
            Task.WaitAll();
            Console.WriteLine("Матрица C: ");
            m3.Print();
            m3.ArrayToFile("..//..//..//..//matrix3.txt");
            Console.WriteLine("Перемножение матриц выполнено!");
            Console.ReadKey();
        }

        static int TaskMethodMulti(int[] row, int[] column)
        {
            int sum = 0;
            for (int i = 0; i < row.Length; i++)
                sum += row[i] * column[i];
            return sum;
        }
    }
}
