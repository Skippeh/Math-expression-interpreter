using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using RPNLib;

namespace Math_Tokenizer
{
    public class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            Console.WriteLine("RPN parser (shunting-yard algorithm) and RPN interpreter written in C#.\n");

            var parser = new RPNParser();
            while (true)
            {
                try
                {
                    Console.Write(">");
                    var input = Console.ReadLine();

                    if (input == null || input.ToLower() == "exit")
                        return;

                    parser.Expression = input;
                    Console.WriteLine(" " + parser.Calculate());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Invalid expression. (" + ex.Message + ")");
                    Console.WriteLine(ex);
                }

                Console.WriteLine();
            }
        }
    }
}