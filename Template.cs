using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace testmatching
{
    public class Template
    {
        public Template(string type, string template)
        {
            this.type = type;
            this.template = new Regex(template);
        }
        
        public string type;
        public Regex template;
        
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
                        var element = new Template((line.Split(';')[0]), (line.Split(';')[1].ToLower()));
                        listOfTemplates.Add(element);
                    }
                }
            }
            return listOfTemplates;
        }
    }
}