using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MergeTextLib;
using Ninject;

namespace MergeTextConsole
{
    class Program
    {
        private static IKernel Kernel = new StandardKernel(new BaseNinjectModule());
        static void Main(string[] args)
        {
            if (Check(args))
            {
                List<string> origText = GetText(args[0]);
                List<string> newText1 = GetText(args[1]);
                List<string> newText2 = GetText(args[2]);

                IMergeTextFacade facade = Kernel.Get<IMergeTextFacade>();

                var result = facade.GetMergedText(
                    origText, 
                    newText1, 
                    newText2, 
                    Path.GetFileName(args[1]),
                    Path.GetFileName(args[2]));

                var tmpPath = GetResultFilePath(args[0]);
                WriteAllLines(tmpPath, result.ToArray());
            }

            Console.WriteLine("Done.");
            Console.ReadLine();
        }

        public static void WriteAllLines(string path, params string[] lines)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            if (lines == null)
                throw new ArgumentNullException("lines");

            using (var stream = File.OpenWrite(path))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                if (lines.Length > 0)
                {
                    for (int i = 0; i < lines.Length - 1; i++)
                    {
                        writer.WriteLine(lines[i]);
                    }
                    writer.Write(lines[lines.Length - 1]);
                }
            }
        }

        private static string GetResultFilePath(string path)
        {
            if (!File.Exists(@path))
            {
                throw new ArgumentException(String.Format("Файл '{0}' не найден.", @path));
            }

            var fileName = Path.GetFileNameWithoutExtension(@path) + "_merge" + Path.GetExtension(@path);
            return Path.Combine(Path.GetDirectoryName(@path), fileName);
        }

        private static List<string> GetText(string path)
        {
            if (!File.Exists(@path))
            {
                throw new ArgumentException(String.Format("Файл '{0}' не найден.", @path));
            }

            return File.ReadAllLines(@path).ToList();
        }

        static bool Check(string[] args)
        {
            bool result = true;

            if (args.Count() < 3)
            {
                Console.WriteLine("Неверное число аргументов!");
                Console.WriteLine("Необходимо:");
                Console.WriteLine("1) Путь к исходному файлу");
                Console.WriteLine("2) Путь к первому измененному файлу");
                Console.WriteLine("3) Путь ко второму измененному файлу");
                result = false;
            }
            return result;
        }
    }
}
