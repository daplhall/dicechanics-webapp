namespace DieInterpreter;
/*I can likely write the parser better in C++, so might need tot look into using it*/

/*
 * States pass the cargo Back and forth
 * We then create an inheritated Class of Context called TokenizerContext
 * Which allows us to fetch the info
 * For making things private
 * https://stackoverflow.com/questions/13270414/inconsistent-accessibility-base-class-is-less-accessible-than-class
*/

public static class StatementTokenizer
{
        public static List<Instruction> Parse(string statement)
        {
                TokenizerStateData cargo = new(statement);
                StateContext context = new(new NewTokenState(cargo));
                StateMachineLoop.Run(context);
                return ShuntingYard.ReversePolish(cargo.Instructions);
        }
}


internal static class ShuntingYard
{
        public static List<Instruction> ReversePolish(List<Instruction> instructions)
        {
                Stack<Instruction> polish = new();
                Stack<Instruction> yard = new();
                foreach (Instruction inst in instructions) {
                        switch (inst.Type) {
                        case InstructionType.Operator:
                                HandleOperator(inst, ref yard, ref polish);
                                break;
                        case InstructionType.ParenthesesOpen:
                                yard.Push(inst);
                                break;
                        case InstructionType.ParenthesesClosed:
                                FlushParentasis(ref yard, ref polish);
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

        private static void HandleOperator(Instruction inst, ref Stack<Instruction> yard, ref Stack<Instruction> polish)
        {
                if (yard.Count != 0 && yard.Peek().Type == InstructionType.ParenthesesOpen) {
                        yard.Push(inst);
                } else if (yard.Count == 0 || Operators.GetWeight(inst.Symbol) >= Operators.GetWeight(yard.Peek().Symbol)) {
                        yard.Push(inst);
                } else {
                        while (yard.Count != 0) {
                                polish.Push(yard.Pop());
                        }
                }
        }

        private static void FlushParentasis(ref Stack<Instruction> yard, ref Stack<Instruction> polish)
        {

                while (yard.Peek().Type != InstructionType.ParenthesesOpen) {
                        polish.Push(yard.Pop());
                }
                yard.Pop();
                return;
        }
}

internal static class Operators
{
        private readonly static Dictionary<string, int> operations = new() {
                { "+", 1},
                { "-", 1},
                { "*", 2},
                { "/", 2},
                { "d", 3}
        };
        public static bool IsOperator(string charater)
        {
                return operations.ContainsKey(charater);
        }
        public static int GetWeight(string key)
        {
                return operations[key];
        }
}

file class TokenizerStateData(string statement)
{
        public StringPointer Statement { get; set; } = new StringPointer(statement);
        public List<Instruction> Instructions { get; set; } = [];
        //public string buffer = "";
        public Instruction Token = new(InstructionType.Undefined, "");
}

file abstract class TokenizerBaseState(TokenizerStateData cargo) : BaseState
{
        public TokenizerStateData Cargo { get; protected set; } = cargo;

        protected char IncrementStatement()
        {
                Cargo.Statement++;
                return Cargo.Statement.GetCurrent();

        }
        protected char WriteBufferAndIncrement()
        {
                Cargo.Token.Symbol += Cargo.Statement.GetCurrent();
                return IncrementStatement();

        }
}

file class NewTokenState : TokenizerBaseState
{
        public NewTokenState(TokenizerStateData cargo) : base(cargo)
        {
                Type = StateType.Transtistional;
        }

        public override void Update()
        {
                Cargo.Token = new(InstructionType.Undefined, "");
                context_.TranstistionTo(new TypeOfTokenState(Cargo));
        }
        public override void Enter() { }
        public override void Exit() { }
}

file class TypeOfTokenState : TokenizerBaseState
{
        public TypeOfTokenState(TokenizerStateData cargo) : base(cargo)
        {
                Type = StateType.Transtistional;
        }
        public override void Update()
        {
                char c = Cargo.Statement.GetCurrent();
                while (char.IsWhiteSpace(c)) {
                        c = IncrementStatement();
                }
                if (c == '"') {
                        this.context_.TranstistionTo(new StringState(Cargo));
                        return;
                } else if (c == '`') {
                        this.context_.TranstistionTo(new VariableState(Cargo));
                        return;
                } else if (char.IsDigit(c)) {
                        this.context_.TranstistionTo(new NumericalState(Cargo));
                        return;
                } else if (Operators.IsOperator(c.ToString())) {
                        this.context_.TranstistionTo(new OperatorState(Cargo));
                        return;
                } else if (c == '(' || c == ')') {
                        this.context_.TranstistionTo(new ParentasisState(Cargo));
                        return;
                } else {
                        this.context_.TranstistionTo(new ErrorState(Cargo));
                        return;
                }
        }

        public override void Enter() { }
        public override void Exit() { }
}

file class StringState : TokenizerBaseState
{
        public StringState(TokenizerStateData cargo) : base(cargo)
        {
                Type = StateType.Transtistional;
        }

        public override void Update()
        {
                Cargo.Statement++;
                char c = Cargo.Statement.GetCurrent();
                while (c != '"') {
                        c = WriteBufferAndIncrement();
                        if (c == '\0') {
                                context_.TranstistionTo(new ErrorState(Cargo));
                                return;
                        }
                }
                Cargo.Statement++;
                Cargo.Token.Type = InstructionType.LiteralSting;
                context_.TranstistionTo(new CompleteTokenState(Cargo));
        }
        public override void Enter()
        {

        }
        public override void Exit() { }
}

file class ParentasisState : TokenizerBaseState
{
        public ParentasisState(TokenizerStateData cargo) : base(cargo)
        {
                Type = StateType.Transtistional;
        }

        public override void Update()
        {
                char c = Cargo.Statement.GetCurrent(); ;
                switch (c) {
                case '(':
                        Cargo.Token.Type = InstructionType.ParenthesesOpen;
                        break;
                case ')':
                        Cargo.Token.Type = InstructionType.ParenthesesClosed;
                        break;
                }
                IncrementStatement();
                context_.TranstistionTo(new CompleteTokenState(Cargo));

        }
        public override void Enter() { }
        public override void Exit() { }
}

file class NumericalState : TokenizerBaseState
{
        public NumericalState(TokenizerStateData cargo) : base(cargo)

        {
                Type = StateType.Transtistional;
        }
        public override void Update()
        {
                char c = Cargo.Statement.GetCurrent();
                while (c != '.' && char.IsDigit(c)) {
                        c = WriteBufferAndIncrement();
                }
                Cargo.Token.Type = InstructionType.LiteralInteger;
                if (c == '.') {
                        c = WriteBufferAndIncrement();
                        while (char.IsDigit(c)) {
                                c = WriteBufferAndIncrement();
                        }
                        Cargo.Token.Type = InstructionType.LiteralDouble;
                }

                context_.TranstistionTo(new CompleteTokenState(Cargo));
        }
        public override void Enter() { }
        public override void Exit() { }
}
file class VariableState : TokenizerBaseState
{
        public VariableState(TokenizerStateData cargo) : base(cargo)

        {
                Type = StateType.Transtistional;
        }
        public override void Update()
        {
                Cargo.Statement++;
                char c = Cargo.Statement.GetCurrent();
                while (c != '`') {
                        c = WriteBufferAndIncrement();
                        if (c == '\0') {
                                context_.TranstistionTo(new ErrorState(Cargo));
                                return;
                        }
                }
                Cargo.Statement++;
                Cargo.Token.Type = InstructionType.Variable;
                context_.TranstistionTo(new CompleteTokenState(Cargo));
        }
        public override void Enter() { }
        public override void Exit() { }
}

file class OperatorState : TokenizerBaseState
{
        public OperatorState(TokenizerStateData cargo) : base(cargo)
        {
                Type = StateType.Transtistional;
        }
        public override void Update()
        {
                char c = Cargo.Statement.GetCurrent();
                while (Operators.IsOperator(Cargo.Token.Symbol + c)) {
                        c = WriteBufferAndIncrement();

                }
                Cargo.Token.Type = InstructionType.Operator;
                context_.TranstistionTo(new CompleteTokenState(Cargo));
        }
        public override void Enter() { }
        public override void Exit() { }
}

file class CompleteTokenState : TokenizerBaseState
{
        public CompleteTokenState(TokenizerStateData cargo) : base(cargo)
        {

                Type = StateType.Transtistional;
        }
        public override void Update()
        {
                Cargo.Instructions.Add(Cargo.Token);
                if (Cargo.Statement.GetCurrent() == '\0') {
                        context_.TranstistionTo(new ExitState(Cargo));
                } else {
                        context_.TranstistionTo(new NewTokenState(Cargo));
                }
        }
        public override void Enter() { }
        public override void Exit() { }
}
file class ExitState : TokenizerBaseState
{
        public ExitState(TokenizerStateData cargo) : base(cargo)
        {

                Type = StateType.Exit;
        }
        public override void Update()
        {
        }
        public override void Enter() { }
        public override void Exit() { }
}
file class ErrorState : TokenizerBaseState
{
        public ErrorState(TokenizerStateData cargo) : base(cargo)
        {

                Type = StateType.Exit;
        }
        public override void Update()
        {
        }
        public override void Enter() { }
        public override void Exit() { }
}