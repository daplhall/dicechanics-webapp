using PythonSerivce.Service.Interpreter;
public class Instruction(InstructionType type, string value)
{
        public InstructionType Type = type;
        public string Value = value;
}
public enum InstructionType
{
        LiteralNumeric,
        Operator,
        Undefined,
}