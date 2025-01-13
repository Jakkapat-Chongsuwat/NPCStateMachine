using UnityEngine;

namespace Jakkapat.StateMachine.Core
{
    /// <summary>
    /// A ScriptableObject-based state that can nest a sub-machine.
    /// Each method returns a StateKey. The sub-machine might also manage states/keys internally.
    /// </summary>
    public abstract class NestableScriptableState : ScriptableState
    {
        [Header("Sub-Machine for Nested Logic")]
        public ScriptableStateMachine subMachine;

        private bool _initialized;

        public override StateKey OnEnter(BaseContext context, StateKey fromKey)
        {
            // call base logic if you want
            var resultKey = base.OnEnter(context, fromKey);

            if (!_initialized && subMachine != null)
            {
                subMachine.Init(context);
                _initialized = true;
            }
            // Possibly return a different key? Usually we just return our own
            return this.key;
        }

        public override StateKey OnUpdate(BaseContext context)
        {
            var resultKey = base.OnUpdate(context);
            subMachine?.Tick();
            // By default, we remain in the same key
            return this.key;
        }

        public override StateKey OnExit(BaseContext context, StateKey toKey)
        {
            // optionally exit sub-machine's current state
            if (subMachine != null)
            {
                subMachine.ExitCurrentState(toKey);
            }

            var resultKey = base.OnExit(context, toKey);
            // remain in the same key or something else
            return this.key;
        }
    }
}
