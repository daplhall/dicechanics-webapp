
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.SignalR;

namespace PythonSerivce.Service;


public interface IStateContext
{
        public void TranstistionTo(IState state);
        public void Step();
        public StateType GetStateType();
}

public class EmptyStateContext : IStateContext
{
        public void TranstistionTo(IState state) { }

        public void Step() { }
        public StateType GetStateType()
        {
                return StateType.Exit;
        }
}

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
                this.state_.SetContext(this);
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

public enum StateType
{
        Transtistional,
        Exit,
}
public interface IState
{
        public StateType Type { get; }
        public void SetContext(IStateContext context);
        public void Enter();
        public void Update();
        public void Exit();
}

public abstract class BaseState : IState
{
        protected IStateContext context_ = new EmptyStateContext();

        public StateType Type { get; protected set; }

        public void SetContext(IStateContext context)
        {
                this.context_ = context;
        }

        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
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
