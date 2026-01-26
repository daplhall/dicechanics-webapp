using PythonSerivce.Service.Interpreter;
public class Instruction(InstructionType type, string symbol)
{
        public InstructionType Type = type;
        public string Symbol = symbol;

        public override string ToString()
        {
                return $"[{Type}] {Symbol}";
        }
}
public enum InstructionType
{
        LiteralInteger,
        LiteralDouble,
        LiteralSting,
        Operator,
        Variable,
        ParenthesesOpen,
        ParenthesesClosed,
        Undefined,
}