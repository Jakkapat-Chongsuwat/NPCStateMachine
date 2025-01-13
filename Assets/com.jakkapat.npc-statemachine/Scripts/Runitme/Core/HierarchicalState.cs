using UnityEngine;

namespace Jakkapat.ToppuFSM.Core
{
    /// <summary>
    /// A base class for hierarchical (nested) states that contain a sub-state machine.
    /// </summary>
    public class HierarchicalState<TContext> : BaseState<TContext>
    {
        protected StateMachine<TContext> subStateMachine;

        // The default sub-state to enter upon OnEnter.
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

            // The sub-state machine uses the same Context as the parent.
            subStateMachine = new StateMachine<TContext>(
                parentStateMachine.Context,
                null // We'll initialize it in OnEnter()
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
            // Each frame, update the nested sub-state machine
            subStateMachine.Update();
        }

        public override void OnExit()
        {
            // If you want, you could forcibly call subStateMachine.CurrentState?.OnExit()
            // or do subStateMachine.ChangeState(null) before leaving.
            base.OnExit();
        }
    }
}
