using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace Inline2018
{
    class Program
    {
        static void Main(string[] args)
        {
            Inline2018main main = new Inline2018main();
            string path = @"C:\Users\acobo\Desktop\Text.txt";
            main.Run(path);
            main.Out();
            Console.ReadKey();
        }
    }
}
