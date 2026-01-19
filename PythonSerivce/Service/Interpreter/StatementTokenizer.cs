using PythonSerivce.Service;
using PythonSerivce.Service.Interpreter;
namespace PythonSerivce.Service.Interpreter;

/*I can likely write the parser better in C++, so might need tot look into using it*/

/*
 * States pass the cargo Back and forth
 * We then create an inheritated Class of Context called TokenizerContext
 * Which allows us to fetch the info
*/

interface IStatementTokenizer
{

}

public class Instruction(string type, string value)
{
        public string Type = type;
        public string Value = value;

}


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

public class StatementTokenizerCargo(string statement)
{
        public StringPointer Statement { get; set; } = new StringPointer(statement);
        public List<Instruction> Instructions { get; set; } = [];
        public string buffer = "";
}

public abstract class TokenizerBaseState(StatementTokenizerCargo cargo) : BaseState
{
        public StatementTokenizerCargo Cargo { get; protected set; } = cargo;
}

public class NewTokenState : TokenizerBaseState
{
        public NewTokenState(StatementTokenizerCargo cargo) : base(cargo)
        {
                Type = StateType.Transtistional;
        }
        public override void Update()
        {
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

public class ParameterState : TokenizerBaseState
{
        public ParameterState(StatementTokenizerCargo cargo) : base(cargo)

        {
                Type = StateType.Transtistional;
        }
        public override void Update()
        {
                char c = Cargo.Statement.GetCurrent();
                while (char.IsLetterOrDigit(c)) {
                        Cargo.buffer += c;
                        Cargo.Statement++;
                        c = Cargo.Statement.GetCurrent();
                }
                context_.TranstistionTo(new CompleteTokenState(Cargo));
        }
        public override void Enter() { }
        public override void Exit() { }
}

public class OperatorState : TokenizerBaseState
{
        public OperatorState(StatementTokenizerCargo cargo) : base(cargo)
        {
                Type = StateType.Transtistional;
        }
        public override void Update()
        {
                char c = Cargo.Statement.GetCurrent();
                while (Operators.IsOperator(c.ToString())) {
                        Cargo.buffer += c;
                        Cargo.Statement++;
                        c = Cargo.Statement.GetCurrent();
                }
                context_.TranstistionTo(new CompleteTokenState(Cargo));
        }
        public override void Enter() { }
        public override void Exit() { }
}

public class CompleteTokenState : TokenizerBaseState
{
        public CompleteTokenState(StatementTokenizerCargo cargo) : base(cargo)
        {

                Type = StateType.Transtistional;
        }
        public override void Update()
        {
                Cargo.Instructions.Add(new Instruction("Param", Cargo.buffer));
                Cargo.buffer = "";
                if (Cargo.Statement.GetCurrent() == '\0') {
                        context_.TranstistionTo(new ExitState(Cargo));
                } else {
                        context_.TranstistionTo(new NewTokenState(Cargo));
                }
        }
        public override void Enter() { }
        public override void Exit() { }
}
public class ExitState : TokenizerBaseState
{
        public ExitState(StatementTokenizerCargo cargo) : base(cargo)
        {

                Type = StateType.Exit;
        }
        public override void Update()
        {
        }
        public override void Enter() { }
        public override void Exit() { }
}
public class ErrorState : TokenizerBaseState
{
        public ErrorState(StatementTokenizerCargo cargo) : base(cargo)
        {

                Type = StateType.Exit;
        }
        public override void Update()
        {
        }
        public override void Enter() { }
        public override void Exit() { }
}