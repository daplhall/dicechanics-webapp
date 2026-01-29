namespace DieInterpreter;

internal interface IOperators
{
        public static abstract bool IsOperator(string charater);
}

internal interface IOperator
{
        public int Weight { get; }
        public string Symbol { get; }
}


