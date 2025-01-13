namespace Jakkapat.ToppuFSM.Core
{
    public interface ITransition<TContext>
    {
        IState<TContext> FromState { get; }
        IState<TContext> ToState { get; }
        bool ShouldTransition(TContext context);
    }
}
