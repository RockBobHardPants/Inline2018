using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;


namespace Inline2018
{
    class Inline2018main
    {
        SyntaxClass syntax = new SyntaxClass();
        Parser parser = new Parser();
        List<string> outLineList = new List<string>();
        Dictionary<string, string> variables = new Dictionary<string, string>();
        Dictionary<string, string> types = new Dictionary<string, string>();

        int GetInputPriority(string checkOperator)
        {
            switch (checkOperator)
            {
                case "(":
                    return 6;
                case "*":
                    return 3;
                case "/":
                    return 3;
                case "%":
                    return 3;
                case "+":
                    return 2;
                case "-":
                    return 2;
                case ")":
                    return 1;
            }
            return -1;
        }

        int GetStackPriority(string checkOperator)
        {
            switch (checkOperator)
            {
                case "(":
                    return 0;
                case "*":
                    return 3;
                case "/":
                    return 3;
                case "%":
                    return 3;
                case "+":
                    return 2;
                case "-":
                    return 2;
            }
            return -1;
        }

        bool IsOperator(string check)
        {
            if (check == "+" || check == "*" || check == "/" || check == "-" || check == "%")
                return true;
            return false;
        }

        string InfixToPostfix(string infix)
        {
            Regex numberRegex = new Regex(@"[+|-]?[0-9]+");
            Regex itemRegex = new Regex(@"\S+");
            MatchCollection itemCollection = itemRegex.Matches(infix);
            string outputString = "";
            Stack<string> operatorStack = new Stack<string>();
            int rank = 0;
            try
            {
                foreach (var item in itemCollection)
                {
                    string itemString = item.ToString();
                    if (numberRegex.IsMatch(itemString))
                    {
                        outputString += itemString + " ";
                        rank++;
                        continue;
                    }
                    if (itemString == "(")
                    {
                        operatorStack.Push(itemString);
                        continue;
                    }
                    if (itemString == ")")
                    {
                        do
                        {
                            if (operatorStack.Peek() == "(")
                            {
                                operatorStack.Pop();
                                break;
                            }
                            outputString += operatorStack.Pop() + " ";
                            rank--;
                        }
                        while (operatorStack.Count != 0);
                        continue;
                    }
                    if (IsOperator(itemString))
                    {

                        while (operatorStack.Count != 0 && GetInputPriority(itemString) <= GetStackPriority(operatorStack.Peek()))
                        {
                            outputString += operatorStack.Pop() + " ";
                            rank--;
                        }
                        operatorStack.Push(itemString);
                    }
                }
                while (operatorStack.Count != 0)
                {
                    outputString += operatorStack.Pop();
                    rank--;
                }
                if (rank != 1)
                {
                    Console.WriteLine("Expression is invalid!");
                    return "Invalid Expression";
                }
                return outputString;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return "Invalid expression";
            }
        }

        double DoTheMath(double firstOperand, double secondOperand, string operation)
        {
            try
            {
                switch (operation)
                {
                    case "*":
                        return firstOperand * secondOperand;
                    case "/":
                        return firstOperand / secondOperand;
                    case "+":
                        return firstOperand + secondOperand;
                    case "-":
                        return firstOperand - secondOperand;
                    case "%":
                        return firstOperand % secondOperand;
                    default:
                        throw new InvalidOperationException();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return -1000000000000000;
            }
        }

        string EvaluateExpression(string expression)
        {
            Stack<double> operandStack = new Stack<double>();
            Regex numberRegex = new Regex(@"[+|-]?[0-9]+");
            Regex itemRegex = new Regex(@"\S+");
            MatchCollection itemCollection = itemRegex.Matches(expression);
            try
            {
                foreach (var item in itemCollection)
                {
                    string itemString = item.ToString();
                    if (numberRegex.IsMatch(itemString))
                    {
                        double operand = Convert.ToDouble(itemString);
                        operandStack.Push(operand);
                    }
                    if (IsOperator(itemString))
                    {
                        double secondOperand = operandStack.Pop();
                        double firstOperand = operandStack.Pop();
                        double returnedValue = DoTheMath(firstOperand, secondOperand, itemString);
                        operandStack.Push(returnedValue);
                    }
                }
                return operandStack.Pop().ToString();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return "error";
            }
        }

        bool CheckTagsNumber(string input)
        {
            Regex openTagsRegex = new Regex(@"\<all_caps\>");
            Regex closedTagsRegex = new Regex(@"\<\/all_caps\>");
            MatchCollection openTagsCollection = openTagsRegex.Matches(input);
            MatchCollection closedTagsCollection = closedTagsRegex.Matches(input);
            if (openTagsCollection.Count == closedTagsCollection.Count)
                return true;
            else
                return false;
        }

        string DoTheCaps(string input)
        {
            Regex regex = new Regex(@"(\<all_caps\>)+(?'string'[A-z 0-9 .#$_]*)(\<\/all_caps\>)+");
            MatchCollection matchCollection = regex.Matches(input);
            try
            {
                foreach (var match in matchCollection)
                {
                    if (!CheckTagsNumber(match.ToString()))
                    {
                        Console.WriteLine("Invalid tags!");
                        //string oldBadTagsString = match.ToString();
                        //string badTagsString = match.ToString();
                        //badTagsString = badTagsString.Replace("<all_caps>", "");
                        //badTagsString = badTagsString.Replace("</all_caps>", "");
                        //input = input.Replace(oldBadTagsString, badTagsString);
                        //continue;
                        throw new InvalidOperationException();
                    }
                    string uppercaseString = match.ToString();
                    uppercaseString = uppercaseString.Replace("<all_caps>", "");
                    uppercaseString = uppercaseString.Replace("</all_caps>", "");
                    uppercaseString = uppercaseString.ToUpper();
                    string lowercaseString = match.ToString();
                    input = input.Replace(lowercaseString, uppercaseString);
                }
            }
            catch(Exception exception)
            {
                Console.WriteLine("Error " + exception.Message);
                return "";
            }
            return input;
        }

        string Concatenate(string input)
        {
            Regex regex = new Regex("\"(?'text'.*?)\"");
            MatchCollection matchCollection = regex.Matches(input);
            input = input.Replace(" + ", "");
            input = input.Replace("\"", "");
            return input;
        }

        bool IsVariableDeclared(string input)
        {
            return variables.ContainsKey(input);
        }

        bool IsTextAdd(string input)
        {
            Regex regexString = new Regex(@"^(\""(?'string'[A-z 0-9 .#$_]*)\"")+");
            if (regexString.IsMatch(input))
                return true;
            else
                return false;
        }

        string VariableAdd(string input)
        {
            Regex regex = new Regex(@"\[(?'variable'[A-z]+[0-9]*[_\.\$#]*?)\]");
            MatchCollection matchCollection = regex.Matches(input);
            if (matchCollection.Count != 0)
            {
                try
                {
                    foreach (var x in matchCollection)
                    {
                        if (!regex.IsMatch(input))
                            break;
                        string variable = regex.Match(input).Groups["variable"].ToString();
                        string oldExpression = regex.Match(input).Value.ToString();
                        if (IsVariableDeclared(variable))
                        {
                            string newExpression = variables[variable].ToString();
                            input = input.Replace(oldExpression, newExpression);
                        }
                        else
                        {
                            Console.WriteLine("Variable: " + variable + " was not declared!");
                            input = input.Replace(oldExpression, "");
                        }
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
            return input;
        }

        string VarDeclaration(string input)
        {
            Regex regex = new Regex(@"^\[(?'variable'.*)\] *= *(?'expression'.*)$");
            string variable = regex.Match(input).Groups["variable"].ToString();
            string expression = regex.Match(input).Groups["expression"].ToString();
            expression = VariableAdd(expression);
            if (expression == "")
                return expression;
            if (!IsVariableDeclared(variable))
            {
                if (IsTextAdd(expression) && !syntax.IsConcatenation(expression))
                {
                    variables.Add(variable, parser.StringParse(expression));
                    types.Add(variable, "string");
                }
                else if (!syntax.IsContainingLetters(expression))
                {
                    string postfixExpression = InfixToPostfix(expression);
                    variables.Add(variable, EvaluateExpression(postfixExpression));
                    types.Add(variable, "numerical");
                }
                else if (syntax.IsConcatenation(expression))
                {
                    string concatenatedString = Concatenate(expression);
                    variables.Add(variable, concatenatedString);
                    types.Add(variable, "string");
                }
                else
                {
                    Console.WriteLine("Invalid declaration of variable: " + variable);
                    return input;
                }
            }
            else
            {
                string oldVariable = variables[variable];
                string newValue;
                if (types[variable] == "string" && syntax.IsContainingLetters(expression))
                {
                    if (IsTextAdd(expression))
                    {
                        newValue = parser.StringParse(expression);
                        variables[variable] = newValue;
                    }
                    else if (syntax.IsConcatenation(expression))
                    {
                        newValue = Concatenate(expression);
                        variables[variable] = newValue;
                    }
                }
                else if (types[variable] == "numerical" && !syntax.IsContainingLetters(expression))
                {
                    newValue = EvaluateExpression(InfixToPostfix(expression));
                    variables[variable] = newValue;
                }
                else
                {
                    Console.WriteLine("Variable: " + variable + " is wrong type, and can not be declared!");
                }
                return input;
            }
            return input;
        }

        string ReplaceVariables(string input)
        {
            Regex regex = new Regex(@"=\[(?'variable'[A-z 0-9]+?)\]");
            try
            {
                MatchCollection matchCollection = regex.Matches(input);
                foreach (var x in matchCollection)
                {
                    string oldValue = regex.Match(input).Value.ToString();
                    string variableName = regex.Match(input).Groups["variable"].ToString();
                    if (IsVariableDeclared(variableName))
                    {
                        string newValue = variables[variableName].ToString();
                        input = input.Replace(oldValue, newValue);
                    }
                    else
                    {
                        Console.WriteLine("Variable: " + variableName + " is not decleared");
                        input = input.Replace(oldValue, "");
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            return input;
        }

        string InlineBlock(string input)
        {
            Regex regex = new Regex(@"\@{ (?'expression'.+) }");
            string inlineExpression = regex.Match(input).ToString();
            VarDeclaration(regex.Match(input).Groups["expression"].ToString());
            input = input.Replace(inlineExpression, "");
            return input;
        }

        public void Out()
        {
            Console.WriteLine();
            Console.WriteLine("Inline2018 out: ");
            Console.WriteLine();
            foreach (var line in outLineList)
                Console.WriteLine(line);
            Console.WriteLine();
            Console.WriteLine("Press any key to continue ");
        }
        
        public bool Run(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    Console.WriteLine("File does not exist!");
                    return false;
                }
                using (StreamReader reader = new StreamReader(path))
                {
                    while (reader.Peek() > -1)
                    {
                        string line = reader.ReadLine();
                        if (!syntax.IsCharValid(line))
                            return false;
                        if (syntax.IsInline(line))
                        {
                            line = InlineBlock(line);
                        }
                        if (syntax.IsInclude(line))
                        {
                            Regex regex = new Regex(@"(\""include\"") (?'path'\"".+\"")");
                            string newPath = regex.Match(line).Groups["path"].ToString();
                            newPath = newPath.Replace("\"", "");
                            Run(newPath);
                            continue;
                        }
                        if (syntax.CheckTags(line))
                        {
                            line = DoTheCaps(line);
                        }
                        if (syntax.IsDeclaration(line))
                        {
                            VarDeclaration(line);
                        }
                        else
                        {
                            string newLine = ReplaceVariables(line);
                            outLineList.Add(newLine);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Process failed: {0}", exception.ToString());
            }
            return true;
        }
    }
}
