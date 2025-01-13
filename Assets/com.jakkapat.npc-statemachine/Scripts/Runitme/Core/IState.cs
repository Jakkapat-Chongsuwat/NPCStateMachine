using System;

namespace Jakkapat.StateMachine.Core
{
    public interface IState<TContext, TStateEnum>
           where TStateEnum : struct, Enum
    {
        TStateEnum ID { get; }
        void EnterState(TContext context, TStateEnum fromState);
        void UpdateState(TContext context);
        void ExitState(TContext context, TStateEnum toState);
    }
}