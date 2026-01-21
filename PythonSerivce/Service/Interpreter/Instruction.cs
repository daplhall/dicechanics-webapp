using PythonSerivce.Service.Interpreter;
public class Instruction(InstructionType type, string value)
{
        public InstructionType Type = type;
        public string Value = value;

        public override string ToString()
        {
                return $"[{Type}] {Value}";
        }
}
public enum InstructionType
{
        LiteralNumeric,
        LiteralSting,
        Operator,
        Variable,
        ParenthesesOpen,
        ParenthesesClosed,
        Undefined,
}