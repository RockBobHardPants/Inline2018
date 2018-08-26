using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Inline2018
{
    class Parser
    {
        public string StringParse(string input)                                         //String parser
        {
            Regex litertal_str = new Regex("\"(?'text'.*?)\"");                         
            MatchCollection matchCollection = litertal_str.Matches(input);              //Collection of all strings to parse
            foreach (var match in matchCollection)
            {
                string oldText = litertal_str.Match(input).ToString();                  //Old string to replace
                string text = litertal_str.Match(input).Groups["text"].ToString();      //Getting string from regex match
                input = input.Replace(oldText, text);
            }
            return input;
        }
    }
}
