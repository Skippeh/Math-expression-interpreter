using System;
using System.Collections.Generic;
using Math_Tokenizer.TokenOperands;

namespace Math_Tokenizer
{
    public class RPNInterpreter
    {
        public List<Operand> Tokens { get; private set; }
        public Dictionary<string, Func<double, double>> Functions { get; private set; } 

        public RPNInterpreter(List<Operand> tokens, Dictionary<string, Func<double, double>> functions)
        {
            Tokens = tokens;
            Functions = functions;
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
                    stack.Push(Functions[token.Token].Invoke(stack.Pop()));
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