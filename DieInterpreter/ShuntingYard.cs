using System.Reflection.Metadata.Ecma335;

namespace DieInterpreter;

internal class ShuntingYardWeights
{
}
internal class ShuntingYard(IOperators operators)
{
        private readonly IOperators operators = operators;

        public List<Instruction> ReversePolish(List<Instruction> instructions)
        {
                Stack<Instruction> polish = new();
                Stack<Instruction> yard = new();
                foreach (Instruction inst in instructions) {
                        switch (inst.Type) {
                        case InstructionType.Operator:
                                HandleOperator(inst, ref yard, ref polish);
                                break;
                        case InstructionType.Function:
                                HandleOperator(inst, ref yard, ref polish);
                                break;
                        case InstructionType.ParenthesesOpen:
                                yard.Push(inst);
                                break;
                        case InstructionType.ParenthesesClosed:
                                FlushParenthesis(ref yard, ref polish);
                                break;
                        default:
                                polish.Push(inst);
                                break;
                        }
                }
                while (yard.Count != 0) {
                        polish.Push(yard.Pop()); // might be replace with ToArray in a Add Range
                }

                List<Instruction> result = [];
                result.AddRange(polish.Reverse());
                result.AddRange(yard.Reverse());
                return result;
        }

        private void HandleOperator(Instruction inst, ref Stack<Instruction> yard, ref Stack<Instruction> polish)
        {
                if (yard.Count != 0 && yard.Peek().Type == InstructionType.ParenthesesOpen) {
                        yard.Push(inst);
                } else if (yard.Count == 0 || GetWeight(inst.Type, inst.Symbol) >= GetWeight(yard.Peek().Type, yard.Peek().Symbol)) {
                        yard.Push(inst);
                } else {
                        while (yard.Count != 0) {
                                polish.Push(yard.Pop());
                        }
                }
        }

        private void FlushParenthesis(ref Stack<Instruction> yard, ref Stack<Instruction> polish)
        {

                while (yard.Peek().Type != InstructionType.ParenthesesOpen) {
                        polish.Push(yard.Pop());
                }
                yard.Pop();
                return;
        }
        private int GetWeight(InstructionType type, string symbol)
        {
                return type switch {
                        InstructionType.Function => 10000, // Max weight;
                        InstructionType.Operator => operators.GetWeight(symbol),
                        _ => throw new Exception("Type doesn't have a weight"),
                };
        }
}