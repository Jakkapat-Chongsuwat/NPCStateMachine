using System;
using System.Collections.Generic;

namespace Jakkapat.StateMachine.Core
{
    /// <summary>
    /// A strongly-typed state machine that uses TStateEnum
    /// for the ID. Each module can define its own TStateEnum.
    /// </summary>
    public class StateMachine<TContext, TStateEnum>
        where TStateEnum : struct, Enum
    {
        protected readonly Dictionary<TStateEnum, IState<TContext, TStateEnum>> _states
            = new Dictionary<TStateEnum, IState<TContext, TStateEnum>>();

        protected IState<TContext, TStateEnum> _currentState;

        public TContext Context { get; set; }
        public TStateEnum CurrentStateID { get; protected set; }

        public StateMachine() { }
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
            _currentState?.ExitState(Context, newStateID);

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

        // If you want to forcibly exit the current sub-state
        public virtual void ExitCurrentState(TStateEnum toState = default)
        {
            _currentState?.ExitState(Context, toState);
        }
    }
}
