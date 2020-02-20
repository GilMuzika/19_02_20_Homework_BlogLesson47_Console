using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace _19_02_20_Homework_BlogLesson47_Console
{
    class Program
    {
        private static  Task _tsk;
        private static Object _key = new Object();
        private static System.Timers.Timer timer = new System.Timers.Timer();
        
        static async Task Main(string[] args)
        {
            ConsoleColor consForeCol = Console.ForegroundColor;



            timer.Interval = 100;
            timer.Elapsed += (object sender, ElapsedEventArgs e) => 
            {
                Console.ForegroundColor = consForeCol;
                Console.WriteLine("\n . ");
                if (_tsk.IsCompleted) timer.Stop();
            };
            timer.Start();
            string comma = ",";
            string fileName = "textFileWithTicks.txt";
            
            for (int i = 1; i <= 1000; i++)
            {
                
                if (i == 1000) { comma = ""; }
                await WriteorAppend(i, comma, fileName);
            }
            

            
            Console.ReadLine();
        }
        static async Task WriteorAppend(int numFactor, string comma, string fileName)
        {
            
            _tsk = Task.Run(async() => 
            {

                if (!File.Exists(Path.Combine(Directory.GetCurrentDirectory(), fileName)))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    await WriteToFileAsync(Directory.GetCurrentDirectory(), fileName, $"{numFactor} " + comma);
                }

                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    await AppendAsync(Path.Combine(Directory.GetCurrentDirectory(), fileName), $"{numFactor} " + comma);
                }
                
            });
            await Task.WhenAll();
            
            
        }
        static async Task WriteToFileAsync(string pathToFolder, string FileName, string content)
        {
            Thread.Sleep(200);
            Console.WriteLine($"Writing \"{content}\" to the file \"{FileName}\" with a method \"WriteToFileAsync\"");
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(pathToFolder, FileName)))
            {
                await outputFile.WriteAsync(content);
            }
        }
        static async Task AppendAsync(string path, string content)
        {
            Console.WriteLine($"appending a \"{content}\" to the file {Path.GetFileName(path)} with a method \"AppendAsync\"");
            Task appendToFile = Task.Factory.StartNew(() => 
            {
                Thread.Sleep(200);
                lock (_key)
                {
                    using (StreamWriter appendTextSw = File.AppendText(path))
                    {
                        appendTextSw.Write(content);
                    }
                }                
            });
            await appendToFile;



        }
    }
}
