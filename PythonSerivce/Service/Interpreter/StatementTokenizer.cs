using PythonSerivce.Service;
using PythonSerivce.Service.Interpreter;
namespace PythonSerivce.Service.Interpreter;

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
                return cargo.Instructions;
        }
}



/* This could just be the instruction you kno*/
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
                char c = Cargo.Statement.GetCurrent();
                if (char.IsLetterOrDigit(c)) {
                        this.context_.TranstistionTo(new ParameterState(Cargo));
                        return;
                } else if (Operators.IsOperator(c.ToString())) {
                        this.context_.TranstistionTo(new OperatorState(Cargo));
                        return;
                } else {
                        this.context_.TranstistionTo(new ErrorState(Cargo));
                        return;
                }
        }

        public override void Enter() { }
        public override void Exit() { }
}

file class ParameterState : TokenizerBaseState
{
        public ParameterState(TokenizerStateData cargo) : base(cargo)

        {
                Type = StateType.Transtistional;
        }
        public override void Update()
        {
                char c = Cargo.Statement.GetCurrent();
                while (char.IsLetterOrDigit(c)) {
                        Cargo.Token.Value += c;
                        Cargo.Statement++;
                        c = Cargo.Statement.GetCurrent();
                }
                Cargo.Token.Type = InstructionType.LiteralNumeric;
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
                while (Operators.IsOperator(c.ToString())) {
                        Cargo.Token.Value += c;
                        Cargo.Statement++;
                        c = Cargo.Statement.GetCurrent();
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