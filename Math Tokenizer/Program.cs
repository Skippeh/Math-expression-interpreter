using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace Math_Tokenizer
{
    public class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            Console.WriteLine("RPN parser (shunting-yard algorithm) and RPN interpreter written in C#.\n");

            while (true)
            {
                try
                {
                    Console.Write(">");
                    var input = Console.ReadLine();

                    if (input == null || input.ToLower() == "exit")
                        return;

                    var parser = new RPNParser(input);
                    var value = parser.Calculate();
                    Console.WriteLine(" " + value.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Invalid expression.");
                }

                Console.WriteLine();
            }
        }
    }
}