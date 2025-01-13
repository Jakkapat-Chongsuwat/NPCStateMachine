using System;
using System.Collections.Generic;

namespace Jakkapat.StateMachine.Core
{
    /// <summary>
    /// A strongly-typed state machine that uses (TContext, TStateEnum).
    /// TStateEnum must be an enum or struct with Enum constraint.
    /// </summary>
    public class StateMachine<TContext, TStateEnum>
        where TStateEnum : struct, Enum
    {
        protected readonly Dictionary<TStateEnum, IState<TContext, TStateEnum>> _states
            = new Dictionary<TStateEnum, IState<TContext, TStateEnum>>();

        protected IState<TContext, TStateEnum> _currentState;

        public TContext Context { get; set; }
        public TStateEnum CurrentStateID { get; protected set; }

        /// <summary>
        /// Parameterless constructor (you can set .Context after creation).
        /// </summary>
        public StateMachine()
        {
            // no-op
        }

        /// <summary>
        /// Optional convenience constructor that sets the context immediately.
        /// </summary>
        public StateMachine(TContext context)
        {
            Context = context;
        }

        public virtual void AddState(IState<TContext, TStateEnum> state)
        {
            if (state == null) return;
            var id = state.ID;
            if (!_states.ContainsKey(id))
                _states[id] = state;
        }

        public virtual void ChangeState(TStateEnum newStateID)
        {
            var oldStateID = CurrentStateID;

            // Exit old
            _currentState?.ExitState(Context, newStateID);

            // Switch
            if (_states.TryGetValue(newStateID, out var next))
            {
                _currentState = next;
                CurrentStateID = newStateID;
                _currentState.EnterState(Context, oldStateID);
            }
            else
            {
                _currentState = null;
                CurrentStateID = default;
            }
        }

        public virtual void Update()
        {
            _currentState?.UpdateState(Context);
        }

        /// <summary>
        /// Forcibly exit the current state if needed.
        /// </summary>
        public virtual void ExitCurrentState(TStateEnum toState = default)
        {
            _currentState?.ExitState(Context, toState);
        }
    }
}
