/*	* В директории лежат файлы.
По структуре они содержат три числа, разделенные пробелами.
Первое число — целое, обозначает действие: 1 — умножение и 2 — деление.
Остальные два — числа с плавающей точкой.
Задача: написать многопоточное приложение, 
выполняющее эти действия над числами и сохраняющее результат в файл result.dat.
Файлов в директории заведомо много. */

using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace WpfWorkWithFiles
{
    public partial class MainWindow : Window
    {
        public MainWindow() => InitializeComponent();

        static Task<string> CreateFilesAsync(int Count)
        {
            return Task.Run(() =>
            {
                try
                {
                    string path = "..//..//..//Files";
                    DirectoryInfo dirWorkInfo = Directory.CreateDirectory(path);
                    Random rnd = new Random();
                    for (int i = 0; i < Count; i++)
                        File.WriteAllText(path + "//" + (i + 1).ToString() + ".dat", rnd.Next(1, 3).ToString() + " " + rnd.NextDouble() * 100 + " " + rnd.NextDouble() * 10);
                    return "Файлы созданы.";
                }
                catch (Exception exn)
                {
                    return exn.Message;
                }
            });
        }

        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            string[] files = Directory.GetFiles("..//..//..//Files");
            pbTemp.Minimum = 0;
            pbTemp.Maximum = files.Length;
            Task<double>[] tasks = new Task<double>[files.Length];
            for (int i=0; i < tasks.Length; i++)
            {
                pbTemp.Value = i;
                string[] str = File.ReadAllText(files[i]).Split(' ');
                tasks[i] = MethodAsync(int.Parse(str[0]), double.Parse(str[1]), double.Parse(str[2]));
            }

            await Task.WhenAll(tasks);
            pbTemp.Value = 0;

            for (int i=0; i < tasks.Length; i++)
                files[i] = tasks[i].Result.ToString();

            File.WriteAllLines("..//..//..//result.dat", files);
            lStatus.Content = "Работа с файлами завершена.";
        }

        static Task<double> MethodAsync(int operation, double a, double b)
        {
            return (operation == 1) ? Task.Run(() => { return a * b; }) : Task.Run(() => { return a / b; });
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lStatus.Content = await CreateFilesAsync(20000);
            btnStart.IsEnabled = true;
        }
    }
}
