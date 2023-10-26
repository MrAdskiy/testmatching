using System;
using System.IO;
using System.Collections.Generic;

namespace testmatching
{
    class Program
    {
        public static void Main()
        {
            Console.Write("Укажите путь. Если оставить поле пустым, путь будет взят из конфиг файла: ");
            var pathIn = Console.ReadLine();
            if (pathIn == string.Empty)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(@".\config.cfg"))
                        pathIn = sr.ReadLine()?.Split('=')[1] ?? string.Empty;
                    if (pathIn == String.Empty)
                    {
                        Console.WriteLine("Отсутствует путь в конфиг файле\nНажмите любую кнопку для выхода...");
                        Console.ReadKey();
                        Environment.Exit(1);
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Ошибка чтения файла");
                    Console.ReadKey();
                    Environment.Exit(2);
                }
            }
            
            string pathTemplates = @".\templates.cfg";
            string pathOutCorrect = $@".\out_correct_{DateTime.Now.ToString().Split(' ')[0].Replace('.', '_')}.txt";
            string pathOutIncorrect = $@".\out_incorrect_{DateTime.Now.ToString().Split(' ')[0].Replace('.', '_')}.txt";
            string pathToUnknown = $@".\out_unknown_{DateTime.Now.ToString().Split(' ')[0].Replace('.', '_')}.txt";
            string pathToVMR = $@".\ModelMatching_{DateTime.Now.ToString().Split(' ')[0].Replace('.', '_')}.vmr";
            
            //Поиск всех файлов aircrfat.cfg по указанному пути во всех директориях

            string[] aircraftCfgFiles = null;
            try
            {
                aircraftCfgFiles = Directory.GetFiles(pathIn, "aircraft.cfg", SearchOption.AllDirectories);
                Console.WriteLine("Поиск файлов aircraft.cfg по пути: " + pathIn);
                if (aircraftCfgFiles.Length <= 0)
                    Console.WriteLine("Файлов aircraft.cfg по пути: " + pathIn + " не найдено");
            }
            catch (Exception)
            {
                Console.WriteLine("Неверный путь");
                Console.ReadKey();
                Environment.Exit(3);
            }

            // Составляем список всех найденный ливрей и добавляем тип по маске
            var listOfAllLiveries = new List<Airplane>();
            foreach (var el in aircraftCfgFiles)
                listOfAllLiveries.AddRange(Airplane.AddType(Airplane.GetInfo(el), Template.GetTemplates(pathTemplates)));
            
            // Сортировка корректных и некорректных и правил
            var listOfCorrectMatches = new List<Airplane>();
            var listOfIncorrectMatches = new List<Airplane>();
            var listOfUnknown = new List<Airplane>();
            foreach (var el in listOfAllLiveries)
                if (el.callsignPrefix == string.Empty || el.callsignPrefix.Length > 3)
                    listOfIncorrectMatches.Add(el);
                else if (el.typeCode == string.Empty)
                    listOfUnknown.Add(el);
                else 
                    listOfCorrectMatches.Add(el);
            
            
            //Сортировка по одинаковому префиксу и типу
            listOfCorrectMatches = Airplane.Merge(listOfCorrectMatches);
            
            //Составление списков и вывод результатов
            Airplane.PrintToFile (listOfCorrectMatches, pathOutCorrect);
            Airplane.PrintToFile (listOfIncorrectMatches, pathOutIncorrect);
            Airplane.PrintToFile (listOfUnknown, pathToUnknown);
            Airplane.PrintToFile (listOfCorrectMatches, pathToVMR);
            
            Console.Write("Всего найдено файлов aircraft.cfg: " + aircraftCfgFiles.Length + "\n");
            Console.Write("Всего найдено ливрей: " + listOfAllLiveries.Count + "\n");
            Console.Write("Всего создано правил: " + listOfCorrectMatches.Count + "\n");
            Console.Write("Всего нераспознанных правил: " + listOfUnknown.Count + "\n");
            Console.Write("Всего некорректных ливрей: " + listOfIncorrectMatches.Count + "\n");
            Console.Write("Нажмите любую кнопку для завершения...");
            Console.ReadKey();
        }
    }
}