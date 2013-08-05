// Based on http://en.wikipedia.org/wiki/Shunting-yard_algorithm

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using RPNLib.TokenOperands;

namespace RPNLib
{
    public class RPNParser
    {
        public string Expression { get; set; }

        private readonly Dictionary<string, Func<double, double>> functions;

        public RPNParser(string expression)
        {
            Expression = expression;
            functions = new Dictionary<string, Func<double, double>>();

            functions.Add("sin", Math.Sin);
            functions.Add("cos", Math.Cos);
            functions.Add("abs", Math.Abs);
            functions.Add("sqrt", Math.Sqrt);
            functions.Add("atan", Math.Atan);
        }

        public RPNParser() : this("") {}

        public double Calculate()
        {
            var stack = new Stack<Operand>();
            var queue = new Queue<Operand>();

            var tokens = new List<string>(Regex.Split(Expression, @"([\+\-\*\(\)\^\/\ ])").Where(str => str.Trim() != ""));

            foreach(var token in tokens)
            {
                if (IsNumber(token))
                {
                    queue.Enqueue(new Value(token));
                }
                else if (token == "(")
                {
                    stack.Push(new LeftParenthesis(token));
                }
                else if (token == ")")
                {
                    // Until the token at the top of the stack is a left parenthesis, pop operators off the stack onto the output queue.
                    // If the stack runs out without finding a left parenthesis, then there are mismatched parentheses. (handled below)
                    while (!(stack.Peek() is LeftParenthesis))
                    {
                        queue.Enqueue(stack.Pop());
                    }
                    stack.Pop(); // Pop the left parenthesis from the stack, but not onto the output queue.

                    // If the token at the top of the stack is a function token, pop it onto the output queue.
                    if (stack.Peek() is Function)
                    {
                        queue.Enqueue(stack.Pop());
                    }
                }
                else if (IsOperator(token))
                {
                    /*
                        while there is an operator token, o2, at the top of the stack, and
                                either o1 is left-associative and its precedence is equal to that of o2,
                                or o1 has precedence less than that of o2,
                            pop o2 off the stack, onto the output queue;
                        push o1 onto the stack.
                    */

                    while (stack.Count > 0 && stack.Peek() is Operator &&
                        ((IsLeftAssociative(token) && Precedence(token) == Precedence(stack.Peek().Token))
                        || Precedence(token) < Precedence(stack.Peek().Token)))
                    {
                        queue.Enqueue(stack.Pop());
                    }

                    stack.Push(new Operator(token));
                }
                else if (IsFunction(token))
                {
                    stack.Push(new Function(token));
                }
                else
                {
                    // Not part of the algorithm
                    switch (token.ToLower())
                    {
                        default: throw new Exception("Invalid token");
                        case "pi":
                        case "pie": // huehue
                                queue.Enqueue(new Value(Math.PI.ToString()));
                                break;
                    }
                }
            }

            while (stack.Count > 0)
            {
                if (stack.Peek() is LeftParenthesis || stack.Peek() is RightParenthesis)
                {
                    throw new Exception("Unmatched parenthesis found");
                }

                queue.Enqueue(stack.Pop());
            }

            queue.ToList().ForEach(op => Console.Write(op.Token + " "));
            Console.WriteLine();

            var interpreter = new RPNInterpreter(queue.ToList(), functions);
            return interpreter.Interpret();
        }

        private bool IsLeftAssociative(string token)
        {
            return token != "^";
        }

        private bool IsOperator(string token)
        {
            return token == "+" ||
                   token == "-" ||
                   token == "*" ||
                   token == "/" ||
                   token == "^";
        }

        private bool IsFunction(string token)
        {
            return functions.Any(func => func.Key.ToLower() == token.ToLower());
        }

        private bool IsNumber(string str)
        {
            double result;

            if (double.TryParse(str, NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out result))
                return true;

            return false;
        }

        private int Precedence(string token)
        {
            if (token == "+" || token == "-")
                return 0;

            if (token == "/" || token == "*")
                return 1;

            if (token == "^")
                return 2;

            return int.MaxValue;
        }
    }
}