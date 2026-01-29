using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Swift;
using System.Xml.Serialization;
using DieInterpreter.StateMachine;
/*I can likely write the parser better in C++, so might need tot look into using it*/

/*
 * States pass the cargo Back and forth
 * We then create an inherited Class of Context called TokenizerContext
 * Which allows us to fetch the info
 * For making things private
 * https://stackoverflow.com/questions/13270414/inconsistent-accessibility-base-class-is-less-accessible-than-class
*/
namespace DieInterpreter.Tokenizer;


public static class StatementTokenizer
{
        public static List<Instruction> Parse(string statement)
        {
                Operators operators = new(new Dictionary<string, int>{
                                { "+", 1},
                                { "-", 1},
                                { "*", 2},
                                { "/", 2},
                                { "d", 3}
                        }
                );
                TokenizerData cargo = new(statement);
                TokenizerContext context = new(operators, cargo);
                StateMachineLoop.Run(context);
                return new ShuntingYard(operators).ReversePolish(cargo.Instructions);
        }
}


internal class TokenizerData(string statement)
{
        public StringPointer Statement { get; set; } = new StringPointer(statement);
        public List<Instruction> Instructions { get; set; } = [];
        //public string buffer = "";
        public Instruction Token = new(InstructionType.Undefined, "");
}


internal class TokenizerContext : IStateContext
{
        public IOperators Operators { get; }
        private TokenizerState state;
        public TokenizerContext(IOperators operators, TokenizerData cargo)
        {
                this.Operators = operators;
                state = new NewTokenState(cargo, this);

        }
        public void TransitionTo(TokenizerState state)
        {
                this.state = state;
                state.SetContext(this);
        }

        public void Step()
        {
                state.Enter();
                state.Update();
                state.Exit();
        }
        public StateType GetStateType()
        {
                return state.Type;
        }
}


internal abstract class TokenizerState(TokenizerData cargo) : IState
{
        public TokenizerData Cargo { get; set; } = cargo;
        protected TokenizerContext Context { get; set; } = null!;

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

        public StateType Type { get; protected set; }

        public void SetContext(TokenizerContext context)
        {
                Context = context;
        }


        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
}

internal class NewTokenState : TokenizerState
{
        public NewTokenState(TokenizerData cargo) : base(cargo)
        {
                Type = StateType.Transitional;
        }
        public NewTokenState(TokenizerData cargo, TokenizerContext context) : base(cargo)
        {
                Type = StateType.Transitional;
                context.TransitionTo(this);
        }

        public override void Update()
        {
                Cargo.Token = new(InstructionType.Undefined, "");
                Context.TransitionTo(new TypeOfTokenState(Cargo));
        }
        public override void Enter() { }
        public override void Exit() { }
}

internal class TypeOfTokenState : TokenizerState
{
        public TypeOfTokenState(TokenizerData cargo) : base(cargo)
        {
                Type = StateType.Transitional;
        }
        public override void Update()
        {
                char c = Cargo.Statement.GetCurrent();
                while (char.IsWhiteSpace(c) || c == ',') { // Find a better solution for comma
                        c = IncrementStatement();
                }
                if (c == '"') {
                        Context.TransitionTo(new StringState(Cargo));
                        return;
                } else if (c == '`') {
                        Context.TransitionTo(new VariableState(Cargo));
                        return;
                } else if (char.IsDigit(c)) {
                        Context.TransitionTo(new NumericalState(Cargo));
                        return;
                } else if (Context.Operators.IsOperator(c.ToString())) {
                        Context.TransitionTo(new OperatorState(Cargo));
                        return;
                } else if (c == '(' || c == ')') {
                        Context.TransitionTo(new ParenthesisState(Cargo));
                        return;
                } else if (char.IsLetter(c)) {
                        Context.TransitionTo(new FunctionState(Cargo));
                        return;
                } else {
                        Context.TransitionTo(new ErrorState(Cargo));
                        return;
                }
        }

        public override void Enter() { }
        public override void Exit() { }
}

internal class FunctionState : TokenizerState
{
        public FunctionState(TokenizerData cargo) : base(cargo)
        {
                Type = StateType.Transitional;
        }
        public override void Update()
        {
                char c = Cargo.Statement.GetCurrent();
                while (char.IsLetterOrDigit(c)) {
                        c = WriteBufferAndIncrement();
                }
                Cargo.Token.Type = InstructionType.Function;
                Context.TransitionTo(new CompleteTokenState(Cargo));
        }
        public override void Enter() { }
        public override void Exit() { }
}

internal class StringState : TokenizerState
{
        public StringState(TokenizerData cargo) : base(cargo)
        {
                Type = StateType.Transitional;
        }

        public override void Update()
        {
                Cargo.Statement++;
                char c = Cargo.Statement.GetCurrent();
                while (c != '"') {
                        c = WriteBufferAndIncrement();
                        if (c == '\0') {
                                Context.TransitionTo(new ErrorState(Cargo));
                                return;
                        }
                }
                Cargo.Statement++;
                Cargo.Token.Type = InstructionType.LiteralSting;
                Context.TransitionTo(new CompleteTokenState(Cargo));
        }
        public override void Enter() { }
        public override void Exit() { }
}

internal class ParenthesisState : TokenizerState
{
        public ParenthesisState(TokenizerData cargo) : base(cargo)
        {
                Type = StateType.Transitional;
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
                Context.TransitionTo(new CompleteTokenState(Cargo));

        }
        public override void Enter() { }
        public override void Exit() { }
}

internal class NumericalState : TokenizerState
{
        public NumericalState(TokenizerData cargo) : base(cargo)

        {
                Type = StateType.Transitional;
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

                Context.TransitionTo(new CompleteTokenState(Cargo));
        }
        public override void Enter() { }
        public override void Exit() { }
}
internal class VariableState : TokenizerState
{
        public VariableState(TokenizerData cargo) : base(cargo)

        {
                Type = StateType.Transitional;
        }
        public override void Update()
        {
                Cargo.Statement++;
                char c = Cargo.Statement.GetCurrent();
                while (c != '`') {
                        c = WriteBufferAndIncrement();
                        if (c == '\0') {
                                Context.TransitionTo(new ErrorState(Cargo));
                                return;
                        }
                }
                Cargo.Statement++;
                Cargo.Token.Type = InstructionType.Variable;
                Context.TransitionTo(new CompleteTokenState(Cargo));
        }
        public override void Enter() { }
        public override void Exit() { }
}

internal class OperatorState : TokenizerState
{
        public OperatorState(TokenizerData cargo) : base(cargo)
        {
                Type = StateType.Transitional;
        }
        public override void Update()
        {
                char c = Cargo.Statement.GetCurrent();
                while (Context.Operators.IsOperator(Cargo.Token.Symbol + c)) {
                        c = WriteBufferAndIncrement();

                }
                Cargo.Token.Type = InstructionType.Operator;
                Context.TransitionTo(new CompleteTokenState(Cargo));
        }
        public override void Enter() { }
        public override void Exit() { }
}

internal class CompleteTokenState : TokenizerState
{
        public CompleteTokenState(TokenizerData cargo) : base(cargo)
        {

                Type = StateType.Transitional;
        }
        public override void Update()
        {
                Cargo.Instructions.Add(Cargo.Token);
                if (Cargo.Statement.GetCurrent() == '\0') {
                        Context.TransitionTo(new ExitState(Cargo));
                } else {
                        Context.TransitionTo(new NewTokenState(Cargo));
                }
        }
        public override void Enter() { }
        public override void Exit() { }
}
internal class ExitState : TokenizerState
{
        public ExitState(TokenizerData cargo) : base(cargo)
        {

                Type = StateType.Exit;
        }
        public override void Update()
        {
        }
        public override void Enter() { }
        public override void Exit() { }
}
internal class ErrorState : TokenizerState
{
        public ErrorState(TokenizerData cargo) : base(cargo)
        {

                Type = StateType.Exit;
        }
        public override void Update()
        {
                throw new Exception("Tokeniser Stopped");
        }
        public override void Enter() { }
        public override void Exit() { }
}