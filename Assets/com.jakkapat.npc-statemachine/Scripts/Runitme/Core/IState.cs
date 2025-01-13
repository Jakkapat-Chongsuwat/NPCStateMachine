namespace Jakkapat.StateMachine.Core
{
    /// <summary>
    /// Generic interface for a state in a state machine.
    /// TContext is the context type (e.g. BaseContext or NPCContext).
    /// Now each method returns a StateKey.
    /// </summary>
    public interface IState<TContext>
    {
        // The key or "ID" for this state.
        StateKey Key { get; }

        // Called when entering this state, returning a StateKey if you want 
        // to indicate a new or the same key after enter.
        StateKey EnterState(TContext context, StateKey fromKey);

        // Called each update, returning possibly a next key or the same key.
        StateKey UpdateState(TContext context);

        // Called when exiting this state, returning a key if you want to signal something.
        StateKey ExitState(TContext context, StateKey toKey);
    }
}
