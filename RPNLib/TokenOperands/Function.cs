namespace RPNLib.TokenOperands
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