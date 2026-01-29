namespace DieInterpreter;

public class StringPointer(string text)
{
        private readonly string Statement = text;
        private int Pointer = 0;

        public static StringPointer operator ++(StringPointer operand)
        {
                ++operand.Pointer;
                return operand;
        }

        public static StringPointer operator --(StringPointer operand)
        {
                --operand.Pointer;
                return operand;
        }

        public char GetCurrent()
        {

                return (Pointer < Statement.Length) ? Statement[Pointer] : '\0';
        }

}