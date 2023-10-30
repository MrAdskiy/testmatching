using System;
using System.IO;
using System.Collections.Generic;

namespace testmatching
{
    public struct Template
    {
        public Template(string type, string template)
        {
            this.type = type;
            this.template = template;
        }
        
        public string type;
        public string template;
        
        public static List<Template> GetTemplates (string path)
        {
            var listOfTemplates = new List<Template>();
            using (StreamReader sr = new StreamReader(path))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line != string.Empty)
                    {
                        var splitLines = line.Split(';');
                        var element = new Template((splitLines[0]), (splitLines[1]));
                        listOfTemplates.Add(element);
                    }
                }
            }
            return listOfTemplates;
        }
    }
}