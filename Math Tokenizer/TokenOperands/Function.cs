using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Math_Tokenizer.TokenOperands
{
    public class Function : Operand
    {
        public string FunctionName;

        public Function(string token, string functionName) : base(token)
        {
            FunctionName = functionName;
        }

        public Function(string token) : this(token, token) {}
    }
}