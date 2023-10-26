using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;

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
                }
            }
            
            // string pathIn = @"H:\games\msfs\Community\fsltl-traffic-base\SimObjects\Airplanes";
            string pathTemplates = @".\templates.cfg";
            string pathOutCorrect = @".\out_correct.txt";
            string pathOutIncorrect = @".\out_incorrect.txt";
            string pathToUnknown = @".\out_unknown.txt";
            string pathToVMR = @".\ModelMatching.vmr";
            
            //Поиск всех файлов aircrfat.cfg по указанному пути во всех директориях

            string[] aircraftCfgFiles = null;
            try
            {
                aircraftCfgFiles = Directory.GetFiles(pathIn, "aircraft.cfg", SearchOption.AllDirectories);
                Console.WriteLine("Поиск файлов aircraft.cfg по пути: " + pathIn);
                if (aircraftCfgFiles.Length <= 0)
                {
                    Console.WriteLine("Файлов aircraft.cfg по пути: " + pathIn + " не найдено");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Неверный путь");
                Console.ReadKey();
                throw;
            }
            
            

            // Составляем список всех найденный ливрей
            var listOfAllLiveries = new List<Airplane>();
            for (int i = 0; i < aircraftCfgFiles.Length; i++)
            {
                listOfAllLiveries.AddRange(Airplane.GetInfo(aircraftCfgFiles[i]));
            }
            
            // Проверяем ливреи наа совпадение с типом самолёта и добавляем тип в список всех ливрей
            listOfAllLiveries = Airplane.AddType(listOfAllLiveries, Template.GetTemplates(pathTemplates));
            
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
            listOfCorrectMatches = Airplane.Sort(listOfCorrectMatches);
            
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