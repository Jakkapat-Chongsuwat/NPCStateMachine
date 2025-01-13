using UnityEngine;
using MyGame.StateMachineFramework;

namespace MyGame.NPC
{
    public class PlayerApproachState<TContext> : HierarchicalState<TContext>
        where TContext : INpcContext
    {
        // If you kept the revised constructor with 'defaultSubState' param, do this:
        public PlayerApproachState(
            StateMachine<TContext> parentStateMachine,
            IState<TContext> defaultSubState
        ) : base(parentStateMachine, defaultSubState)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("NPC: Enter PlayerApproach (Hierarchical State)");
        }

        public override void OnExit()
        {
            Debug.Log("NPC: Exit PlayerApproach (Hierarchical State)");
            base.OnExit();
        }
    }
}
