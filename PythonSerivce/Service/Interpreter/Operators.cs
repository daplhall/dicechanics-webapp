namespace PythonSerivce.Service.Interpreter;

public interface IOperators
{
        public static abstract bool IsOperator(string charater);
}


public class Operators : IOperators
{
        private readonly static string[] operations = ["+", "-", "*", "/"];
        public static bool IsOperator(string charater)
        {
                return operations.Contains(charater);
        }
}