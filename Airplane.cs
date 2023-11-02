using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace testmatching
{
    public class Airplane
    {
        public Airplane()
        {
            callsignPrefix = string.Empty;
            typeCode = string.Empty;
            modelName = string.Empty;
        }
        
        public string callsignPrefix;
        public string typeCode;
        public string modelName;

        public static string GetValue(string line)
        {
            line = line.Split('=')[1].Trim();
            if (line.Contains("\""))
                line = line.Split('\"')[1].Trim();
            if (line.Contains(";"))
                line = line.Split(';')[0].Trim();
            return line;
        }
        
        public static List<Airplane> GetInfo(string path)
        {
            var listOfPlanes = new List<Airplane>() ;
            Airplane airplane = null;

            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    Console.WriteLine(path);
                    string line;
                
                    while ((line = sr.ReadLine()) != null)
                    {
                        var lineToLower = line.ToLower();
                        if (lineToLower.Contains("[fltsim.") && !lineToLower.Contains(";"))
                        {
                            airplane = new Airplane();
                            listOfPlanes.Add(airplane);
                        } 
                        else if (lineToLower.Trim().StartsWith("title") && lineToLower.Contains("=") && !lineToLower.Contains("&"))
                        {
                            if (airplane == null)
                            {
                                airplane = new Airplane();
                                listOfPlanes.Add(airplane);
                            }
                            airplane.modelName = GetValue(line);
                        }
                        else if (lineToLower.Trim().StartsWith("icao_airline"))
                        {
                            airplane.callsignPrefix = GetValue(line);
                        }
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Ошибка чтения файла");
                Console.ReadKey();
                Environment.Exit(4);
            }
            return listOfPlanes;
        }

        public static void PrintToFile(List<Airplane> array, string path)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                string template = "    <ModelMatchRule CallsignPrefix=\"{0}\" TypeCode=\"{1}\" ModelName=\"{2}\" />";
                sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<ModelMatchRuleSet>");
                foreach (var plane in array)
                    sw.WriteLine(template, plane.callsignPrefix, plane.typeCode, plane.modelName);
                sw.Write("</ModelMatchRuleSet>");
            }
        }

        public static List<Airplane> Merge(List<Airplane> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = list.Count-1; j > i; j--)
                {
                    if ((list[i].callsignPrefix == list[j].callsignPrefix) && (list[i].callsignPrefix != string.Empty) && (list[i].typeCode == list[j].typeCode) && (list[i].typeCode != string.Empty))
                    {
                        list[i].modelName += ($"//{list[j].modelName}");
                        list.RemoveAt(j);
                    }   
                }
            }
            return list;
        }

        public static List<Airplane> AddType(List<Airplane> listOfLiveries, List<Template> templates)
        {
            Parallel.ForEach(
                listOfLiveries,
                el =>
                {
                    var elToLower = el.modelName.ToLower();
                    foreach (var template in templates)
                    {
                        if (template.template.IsMatch(elToLower))
                        {
                            el.typeCode = template.type.ToUpper();
                            break;
                        }
                    }
                });
            return listOfLiveries;
        }
    }
}