using System;

namespace Jakkapat.StateMachine.Core
{
    /// <summary>
    /// A generic state interface that uses TContext and TStateEnum.
    /// </summary>
    public interface IState<in TContext, TStateEnum>
        where TStateEnum : struct, Enum
    {
        TStateEnum ID { get; }
        void EnterState(TContext context, TStateEnum fromState);
        void UpdateState(TContext context);
        void ExitState(TContext context, TStateEnum toState);
    }
}
