// Based on http://en.wikipedia.org/wiki/Shunting-yard_algorithm

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Math_Tokenizer.TokenOperands;

namespace Math_Tokenizer
{
    public class RPNParser
    {
        public string Expression { get; set; }

        public RPNParser(string expression)
        {
            Expression = expression;
        }

        public RPNParser() { Expression = ""; }

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
                    while (!(stack.Peek() is LeftParenthesis))
                    {
                        queue.Enqueue(stack.Pop());
                    }
                    stack.Pop(); // Pop the left parenthesis from the stack, but not onto the output queue.

                    /* Not doing functions yet
                        If the token at the top of the stack is a function token, pop it onto the output queue.
                        If the stack runs out without finding a left parenthesis, then there are mismatched parentheses. (handled below)
                    */
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
            }

            while (stack.Count > 0)
            {
                while (stack.Count > 0)
                {
                    if (stack.Peek() is LeftParenthesis || stack.Peek() is RightParenthesis)
                    {
                        throw new Exception("Unmatched parenthesis found");
                    }

                    queue.Enqueue(stack.Pop());
                }
            }

            var interpreter = new RPNInterpreter(queue.ToList());
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