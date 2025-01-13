using System.Collections.Generic;

namespace Jakkapat.StateMachine.Core
{
    /// <summary>
    /// A code-based machine that manages states using StateKey.
    /// </summary>
    public class StateMachine<TContext>
    {
        // Instead of Dictionary<StateIDs, IState<TContext>>, 
        // we store Dictionary<StateKey, IState<TContext>>.
        protected readonly Dictionary<StateKey, IState<TContext>> _states
            = new Dictionary<StateKey, IState<TContext>>();

        protected IState<TContext> _currentState;

        public TContext Context { get; set; }

        public StateKey CurrentStateKey { get; protected set; }

        public StateMachine() { }

        public StateMachine(TContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Register a state in the machine. 
        /// The state's Key is used in the dictionary.
        /// </summary>
        public virtual void AddState(IState<TContext> state)
        {
            if (state == null) return;
            var key = state.Key;
            if (!_states.ContainsKey(key))
            {
                _states[key] = state;
            }
        }

        /// <summary>
        /// Transition to the state that has the given Key.
        /// If found, we exit the old state, and enter the new state.
        /// </summary>
        public virtual void ChangeState(StateKey newKey)
        {
            // 1) Exit old
            var oldKey = CurrentStateKey;
            _currentState?.ExitState(Context, newKey);

            // 2) Find the next state by the new Key
            if (_states.TryGetValue(newKey, out var next))
            {
                _currentState = next;
                CurrentStateKey = newKey;

                // 3) Enter the new state
                _currentState.EnterState(Context, oldKey);
            }
            else
            {
                // Not found
                _currentState = null;
                CurrentStateKey = null;
            }
        }

        /// <summary>
        /// Forcibly exit the current state. Pass a 'toKey' if you want to specify 
        /// which Key you're transitioning to, or null if you just want to exit.
        /// </summary>
        public virtual void ExitCurrentState(StateKey toKey = null)
        {
            if (_currentState != null)
            {
                _currentState.ExitState(Context, toKey);
            }
        }

        /// <summary>
        /// The machine calls UpdateState on the current state each frame, 
        /// letting it potentially return a Key to transition to.
        /// </summary>
        public virtual void Update()
        {
            if (_currentState != null)
            {
                // Some states might return a next key for auto-transitions:
                var nextKey = _currentState.UpdateState(Context);

                // If nextKey differs from CurrentStateKey, we can auto-switch:
                if (nextKey != null && nextKey != CurrentStateKey)
                {
                    ChangeState(nextKey);
                }
            }
        }
    }
}
