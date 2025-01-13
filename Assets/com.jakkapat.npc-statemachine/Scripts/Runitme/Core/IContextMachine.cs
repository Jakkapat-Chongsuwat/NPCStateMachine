namespace Jakkapat.StateMachine.Core
{
    /// <summary>
    /// Any Context that implements this interface promises
    /// it has a StateMachine of the same TContext type.
    /// </summary>
    public interface IContextMachine<TContext>
    {
        StateMachine<TContext> StateMachine { get; }
    }
}