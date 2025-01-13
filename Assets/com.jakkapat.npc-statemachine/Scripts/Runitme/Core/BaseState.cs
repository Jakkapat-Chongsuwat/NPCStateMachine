using System;

namespace Jakkapat.StateMachine.Core
{
    public abstract class BaseState<TContext, TStateEnum>
        : IState<TContext, TStateEnum>
        where TStateEnum : struct, Enum
    {
        public abstract TStateEnum ID { get; }

        public virtual void EnterState(TContext context, TStateEnum fromState) { }
        public virtual void UpdateState(TContext context) { }
        public virtual void ExitState(TContext context, TStateEnum toState) { }
    }
}