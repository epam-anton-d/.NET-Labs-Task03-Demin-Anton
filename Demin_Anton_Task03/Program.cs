using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace Demin_Anton_Task03
{
    class Program
    {
        const string separator = @";";

        // Объявление делегатов.
        public delegate string ReadFileDelegate();
        public delegate void SplitAndCountDelegate(string separator, string read, out int summaryOfDigits, out int numberOfStringElements);
        public delegate void FileWriteDelegate(int summaryOfDigits, int numberOfStringElements);
        public delegate void IfRead();

        // Объявление событий.
        public static event IfRead IfReadWriteEvent;

        // Объявление методов-обработчиков событий.

        public static void ShowReadSuccessMessage()
        {
            Console.WriteLine("Чтение из файла успешно завершено!!!");
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        public static void ShowSplitAndCountSuccessMessage()
        {
            Console.WriteLine("Обработка текста успешно завершена!!!");
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        public static void ShowWriteSuccessMessage()
        {
            Console.WriteLine("Запись в файл успешно завершено!!!");
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        // Чтение из файла.
        static string ReadFileFunc()
        {
            StreamReader fl = File.OpenText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "input.txt"));
            //String read = null;
            string read = fl.ReadToEnd();
            fl.Close();
            return read;
        }

        // Обработка текста.
        static void SplitAndCount(string separator, string read, out int summaryOfDigits, out int numberOfStringElements)
        {
            int num = 0,
                numberOfChars = 0;
            summaryOfDigits = 0;
            String[] elements = Regex.Split(read, separator);
            foreach (string item in elements)
            {
                if (item != "")
                {
                        
                    if (int.TryParse(item, out num))
                    {
                        summaryOfDigits += num;
                    }
                        else
                    {
                        numberOfChars += item.Length;
                    }
                }
            }
            numberOfStringElements = numberOfChars;
        }

        // Запись в файл результата обработки текста.
        static void FileWriteFunc(int summaryOfDigits, int numberOfStringElements)
        {
            FileInfo f = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output.txt"));
            StreamWriter w = f.CreateText();
            w.WriteLine("Арифметическая сумма = {0}", summaryOfDigits);
            w.WriteLine("Число элементов = {0}", numberOfStringElements);
            w.Close();
        }

        static void Main(string[] args)
        {
            int summaryOfDigits = 0;
            int numberOfStringElements = 0;
            string read;
            
            // Создание делегатов фунцкций обработки файла.
            ReadFileDelegate readFileDelegate = new ReadFileDelegate(ReadFileFunc);
            SplitAndCountDelegate splitAndCountDelegate = new SplitAndCountDelegate(SplitAndCount);
            FileWriteDelegate fileWriteDelegate = new FileWriteDelegate(FileWriteFunc);

            // Подписка на событие чтение из файла.
            IfReadWriteEvent += ShowReadSuccessMessage;
            // Делегат чтения из файла.
            read = readFileDelegate.Invoke(); 
            if (read != null)
                // Событие чтения из файла.
                IfReadWriteEvent(); 
            // Отписка от события.
            IfReadWriteEvent -= ShowReadSuccessMessage;

            // Подписка на событие окончания обработки файла.
            IfReadWriteEvent += ShowSplitAndCountSuccessMessage;
            // Делегат обработка текста.
            splitAndCountDelegate.Invoke(separator, read, out summaryOfDigits, out numberOfStringElements); 
            // Событие окончания обработки файла.
            IfReadWriteEvent();
            // Отписка от события.
            IfReadWriteEvent -= ShowSplitAndCountSuccessMessage;

            // Подписка на событие записи в файл.
            IfReadWriteEvent += ShowWriteSuccessMessage;
            // Делегат запись в файл.
            fileWriteDelegate.Invoke(summaryOfDigits, numberOfStringElements); 
            // Событие записи в файл.
            IfReadWriteEvent();
            // Отписка от события.
            IfReadWriteEvent -= ShowWriteSuccessMessage;
        }
    }
}
