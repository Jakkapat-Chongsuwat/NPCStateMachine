namespace Jakkapat.StateMachine.Core
{
    using System;

    public interface IContextMachine<TContext, TStateEnum>
        where TStateEnum : struct, Enum
    {
        StateMachine<TContext, TStateEnum> StateMachine { get; }
    }
}