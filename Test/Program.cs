// *Написать приложение, выполняющее парсинг CSV-файла произвольной структуры и сохраняющее его в обычный TXT-файл. 
//  Все операции проходят в потоках. CSV-файл заведомо имеет большой объём.

using System;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.VisualBasic.FileIO;

namespace Test
{
    class MyThread
    {
        readonly Thread tr;
        readonly string fileName;
        readonly string[] stroke;
        readonly string name;
        
        public MyThread(string fileName, string[] stroke, string name)
        {
            this.fileName = fileName;
            this.stroke = stroke;
            this.name = name;
            tr = new Thread(Writing);
            tr.Start();
            tr.Join();
        }
        
        private void Writing()
        {
            FileStream aFile = new FileStream(fileName, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(aFile);
            aFile.Seek(0, SeekOrigin.End);
            for (int i = 0; i < stroke.Length; i++)
            {
                if (!string.IsNullOrEmpty(stroke[i]))
                {
                    DateTime dt = DateTime.Now;
                    sw.WriteLine(stroke[i]);
                    Console.WriteLine("Запись в текстовый файл, поток \"" + name + "\": " + dt.Hour + ":" + dt.Minute + ":" + dt.Second + ":" + dt.Millisecond);
                }
            }
            sw.Close();
        }
    }

    class Program
    {
        static string pathCSV = "yournewstyle.csv";
        static string pathTXT = pathCSV.Substring(0, pathCSV.IndexOf('.')) + ".txt";

        static void Main(string[] args)
        {
            if (File.Exists(pathTXT))
                File.Delete(pathTXT);


            MyThread[] threads = new MyThread[Environment.ProcessorCount];
            int index = 0;
            using (FileStream str = new FileStream(pathCSV, FileMode.Open))
            {
                using (TextFieldParser parser = new TextFieldParser(str, Encoding.UTF8))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(";");
                    while (!parser.EndOfData)
                    {
                        threads[index] = new MyThread(pathTXT, parser.ReadFields(), index.ToString());
                        index = index < threads.Length - 1 ? index + 1 : 0;
                    }
                }
            }
            Console.ReadLine();
        }
    }
}
