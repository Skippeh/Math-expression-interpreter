using System;
using System.Collections.Generic;
using Math_Tokenizer.TokenOperands;

namespace Math_Tokenizer
{
    public class RPNInterpreter
    {
        public List<Operand> Tokens { get; private set; } 

        public RPNInterpreter(List<Operand> tokens)
        {
            Tokens = tokens;
        }

        public double Interpret()
        {
            var stack = new Stack<double>();

            foreach (var token in Tokens)
            {
                if (token is Value)
                {
                    stack.Push(double.Parse(token.Token));
                }
                else
                {
                    if (token.Token == "+" ||
                        token.Token == "-" ||
                        token.Token == "*" ||
                        token.Token == "/" ||
                        token.Token == "^")
                    {
                        var o2 = stack.Pop();
                        var o1 = stack.Pop();

                        switch (token.Token)
                        {
                            default: Console.WriteLine("Invalid RPN"); return 0;
                            case "+":
                                {
                                    stack.Push(o1 + o2);
                                    break;
                                }
                            case "-":
                                {
                                    stack.Push(o1 - o2);
                                    break;
                                }
                            case "*":
                                {
                                    stack.Push(o1 * o2);
                                    break;
                                }
                            case "/":
                                {
                                    stack.Push(o1 / o2);
                                    break;
                                }
                            case "^":
                                {
                                    stack.Push(Math.Pow(o1, o2));
                                    break;
                                }
                        }
                    }
                }
            }

            return stack.Pop();
        }
    }
}