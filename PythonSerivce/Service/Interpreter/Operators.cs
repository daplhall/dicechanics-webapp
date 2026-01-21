namespace PythonSerivce.Service.Interpreter;

internal interface IOperators
{
        public static abstract bool IsOperator(string charater);
}


internal static class Operators
{
        private readonly static string[] operations = ["+", "-", "*", "/", "d"];
        public static bool IsOperator(string charater)
        {
                return operations.Contains(charater);
        }
}
