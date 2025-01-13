using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Jakkapat.StateMachine.Core
{
    [CreateAssetMenu(menuName = "Jakkapat/StateMachine/ScriptableStateMachine", fileName = "ScriptableStateMachine")]
    public class ScriptableStateMachine : ScriptableObject
    {
        [Header("All states used by this machine")]
        public List<ScriptableState> states = new List<ScriptableState>();

        [SerializeField]
        private BaseContext _context;

        [Header("Initial or default state")]
        public ScriptableState startState;

        [System.NonSerialized]
        private ScriptableState _currentState;

        public ScriptableState CurrentState => _currentState;

        /// <summary>
        /// Initialize with a given context (NPCContext, etc.).
        /// </summary>
        public void Init(BaseContext context)
        {
            _context = context;
            _currentState = startState;
            // call OnEnter with a "null" or "empty" fromKey
            if (_currentState)
            {
                _currentState.EnterState(context, null);
            }
        }

        /// <summary>
        /// Called each frame to update current state.
        /// </summary>
        public void Tick()
        {
            if (_currentState)
            {
                // If a new key is returned, we might want to do a transition
                var newKey = _currentState.UpdateState(_context);
                if (newKey && newKey != _currentState.Key)
                {
                    // we want to change state
                    ChangeState(newKey);
                }
            }
        }

        /// <summary>
        /// Changes to a new state by searching in `states`.
        /// Calls OnExit on old, OnEnter on new.
        /// </summary>
        public void ChangeState(StateKey newKey)
        {
            if (!_currentState) return;
            if (_currentState.Key == newKey) return;

            // Exit old
            _currentState.ExitState(_context, newKey);

            // find new
            var found = states.FirstOrDefault(s => s.Key == newKey);
            if (!found)
            {
                Debug.LogWarning($"[ScriptableStateMachine] StateKey '{newKey}' not found in states list!");
                return;
            }

            // enter new
            _currentState = found;
            _currentState.EnterState(_context, _currentState.Key);
        }

        /// <summary>
        /// Forcibly exit current state, e.g. parent state is exiting.
        /// </summary>
        public void ExitCurrentState(StateKey toKey)
        {
            if (_currentState)
            {
                _currentState.ExitState(_context, toKey);
            }
        }
    }
}
