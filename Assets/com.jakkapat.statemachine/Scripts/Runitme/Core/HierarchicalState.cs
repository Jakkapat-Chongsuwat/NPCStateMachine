using UnityEngine;

namespace Jakkapat.ToppuFSM.Core
{
    /// <summary>
    /// A base class for hierarchical (nested) states that contain a sub-state machine.
    /// </summary>
    public class HierarchicalState<TContext> : BaseState<TContext>
    {
        protected StateMachine<TContext> subStateMachine;

        private readonly IState<TContext> defaultSubState;

        /// <summary>
        /// By injecting defaultSubState, we remove the need for an abstract method.
        /// This increases flexibility and reduces code coupling.
        /// </summary>
        public HierarchicalState(
            StateMachine<TContext> parentStateMachine,
            IState<TContext> defaultSubState
        ) : base(parentStateMachine)
        {
            this.defaultSubState = defaultSubState;

            subStateMachine = new StateMachine<TContext>(
                parentStateMachine.Context,
                null
            );
        }

        public override void OnEnter()
        {
            base.OnEnter();

            if (defaultSubState != null)
            {
                subStateMachine.Initialize(defaultSubState);
            }
            else
            {
                Debug.LogWarning("HierarchicalState: No default sub-state was provided.");
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            subStateMachine.Update();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}
