using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace Inline2018
{
    class SyntaxClass
    {
        char[] letters = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q',                         //Lists of all 
                         'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H',                           //allowed letters,
                         'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };                    //numbers, and
        char[] numbers = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };                                                          //special caracters
        char[] specialCharacters = { '_', '.', '$', '#', ' ', '@', '{', '}', '[', ']', '(', ')', '=', '<', '>', ' ', '"', ':', '\\' };  //that can be used
        char[] operators = { '+', '-', '*', '/', '%' };                                                                                 //in Inline2018

        public bool IsCharValid(string input)                                                                                           //Checks for invalid characters in line
        {
            foreach(char x in input)
            {
                if (letters.Contains(x) || numbers.Contains(x) || specialCharacters.Contains(x) || operators.Contains(x))
                    continue;
                else
                {
                    Console.WriteLine(x + " Invalid Character!");
                    return false;
                }
            }
            return true;
        }                                                                                            

        public bool IsContainingLetters(string input)                                                                                   //Check for numerical of string type
        {
            foreach(var x in input)
            {
                if (letters.Contains(x))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsInclude(string input)                                                                                             //Check for "include" keyword
        {
            return Regex.IsMatch(input, @"(\""include\"") (?'path'\"".+\.txt\"")");
        }

        public bool CheckTags(string input)                                                                                             //Check for <all_caps> tags
        {
            return Regex.IsMatch(input, @"(\<all_caps\>)+(?'string'[A-z 0-9 .#$_]*)(\<\/all_caps\>)+");
        }

        public bool IsStringValid(string input)                                                                                         //Checking if there is equal number of quotation marks
        {
            return input.Count(x => x == '"') % 2 == 0;
        }

        public bool IsInline(string input)                                                                                              //Check for inline reserved simbol
        {
            return Regex.IsMatch(input, @"\@{ (?'expression'.+) }");
        }

        public bool IsVarValid(string input)                                                                                            //Check for invalid characters in variable name
        {
            if(!letters.Contains(input[0]))
            {
                Console.WriteLine("Invalid var declaration!");
                return false;
            }
            foreach(char x in input)
            {
                if (letters.Contains(x) || numbers.Contains(x) || specialCharacters.Contains(x))
                    continue;
                else
                {
                    Console.WriteLine(x + " Invalid Character!");
                    return false;
                }
            }
            return true;
        }

        public bool IsDeclaration(string input)                                                                                         //Check if string is variable declaration
        {
            Regex regex = new Regex(@"^\[(?'var'.*)\] *= *(?'rest'.*)$");
            string var = regex.Match(input).Groups["var"].Value.ToString();
            return regex.IsMatch(input) && IsVarValid(var);
        }

        public bool IsConcatenation(string input)                                                                                       //Check if string is concatenation operation
        {
            return Regex.IsMatch(input, @"(\""(?'string'[A-z 0-9]*)\"")+ \+ (?'text'[A-z 0-9 ._#$]*)");
        }

    }
}
