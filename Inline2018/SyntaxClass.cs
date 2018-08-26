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
        char[] letters = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q',
                         'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H',
                         'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        char[] numbers = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        char[] specialCharacters = { '_', '.', '$', '#', ' ', '@', '{', '}', '[', ']', '(', ')', '=', '<', '>', ' ', '"', ':', '\\' };
        char[] operators = { '+', '-', '*', '/', '%' };

        public bool IsCharValid(string input)
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

        public bool IsContainingLetters(string input)
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

        public bool IsInclude(string input)
        {
            return Regex.IsMatch(input, @"(\""include\"") (?'path'\"".+\.txt\"")");
        }

        public bool CheckTags(string input)
        {
            return Regex.IsMatch(input, @"(\<all_caps\>)+(?'string'[A-z 0-9 .#$_]*)(\<\/all_caps\>)+");
        }

        public bool IsInline(string input)
        {
            return Regex.IsMatch(input, @"\@{ (?'expression'.+) }");
        }

        public bool IsVarValid(string input)
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

        public bool IsDeclaration(string input)
        {
            Regex regex = new Regex(@"^\[(?'var'.*)\] *= *(?'rest'.*)$");
            string var = regex.Match(input).Groups["var"].Value.ToString();
            return regex.IsMatch(input) && IsVarValid(var);
        }

        public bool IsConcatenation(string input)
        {
            return Regex.IsMatch(input, @"(\""(?'string'[A-z 0-9]*)\"")+ \+ (?'text'[A-z 0-9 ._#$]*)");
        }

    }
}
