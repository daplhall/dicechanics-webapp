namespace DieInterpreter;

public enum InstructionType
{
        LiteralInteger,
        LiteralDouble,
        LiteralSting,
        Operator,
        Variable,
        ParenthesesOpen,
        ParenthesesClosed,
        Function,
        Undefined,
}
public class Instruction(InstructionType type, string symbol)
{
        public InstructionType Type = type;
        public string Symbol = symbol;

        public override string ToString()
        {
                return $"[{Type}] {Symbol}";
        }
        public bool Equals(Instruction? other)
        {
                if (other is null)
                        return false;
                if (ReferenceEquals(this, other))
                        return true;
                return other.Type == Type && other.Symbol == Symbol;
        }
        public override bool Equals(object? obj) => Equals(obj as Instruction);

        public static bool operator ==(Instruction left, Instruction right)
        {
                return left.Type == right.Type && left.Symbol == right.Symbol;
        }

        // this is second one '!='
        public static bool operator !=(Instruction left, Instruction right)
        {
                return !(left == right);
        }
        public override int GetHashCode()
        {
                return Type.GetHashCode() + Symbol.GetHashCode();
        }

}