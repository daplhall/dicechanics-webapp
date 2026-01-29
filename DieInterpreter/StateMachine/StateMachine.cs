
using System.Diagnostics.CodeAnalysis;


namespace DieInterpreter.StateMachine;

public enum StateType
{
        Transitional,
        Exit,
}
public class StateMachineLoop
{
        public static void Run(IStateContext context)
        {
                while (context.GetStateType() != StateType.Exit) {
                        context.Step();
                }
                context.Step();
                return; // return Cargo; use generic?
        }
}

public interface IStateContext
{
        public void Step();
        public StateType GetStateType();
}



public interface IState
{
        public StateType Type { get; }
        public void Enter();
        public void Update();
        public void Exit();
}


/*
public class StateContext : IStateContext
{
        private IState state_;
        public StateContext(IState state)
        {
                this.TranstistionTo(state);
        }

        [MemberNotNull(nameof(state_))]
        public void TranstistionTo(IState state)
        {
                this.state_ = state;
                this.state_.Context = this;
        }

        public StateType GetStateType()
        {
                return state_.Type;
        }

        public void Step()
        {
                IState currentState = state_;
                currentState.Enter();
                currentState.Update();
                currentState.Exit();
        }
}
*/