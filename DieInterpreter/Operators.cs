namespace DieInterpreter;


public interface IOperators
{

        public bool IsOperator(string character);
        public int GetWeight(string key);
}

internal class Operators(Dictionary<string, int> operators) : IOperators
{
        private readonly Dictionary<string, int> operations = operators;
        public bool IsOperator(string character)
        {
                return operations.ContainsKey(character);
        }
        public int GetWeight(string key)
        {
                return operations[key];
        }
}