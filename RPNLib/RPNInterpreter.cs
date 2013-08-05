using System;
using System.Collections.Generic;
using RPNLib.TokenOperands;

namespace RPNLib
{
    public class RPNInterpreter
    {
        public List<Operand> Tokens { get; private set; }
        private readonly Dictionary<string, Func<double, double>> functions;

        public RPNInterpreter(List<Operand> tokens, Dictionary<string, Func<double, double>> functions)
        {
            Tokens = tokens;
            this.functions = functions;
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
                else if (token is Function)
                {
                    stack.Push(functions[token.Token].Invoke(stack.Pop()));
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
                            default: throw new Exception("Invalid RPN format");
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