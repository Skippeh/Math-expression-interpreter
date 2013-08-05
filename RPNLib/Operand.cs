namespace RPNLib
{
    public abstract class Operand
    {
        public string Token { get; private set; }

        protected Operand(string token)
        {
            Token = token;
        }

        public override string ToString()
        {
            return GetType().Name + ": " + Token;
        }
    }
}