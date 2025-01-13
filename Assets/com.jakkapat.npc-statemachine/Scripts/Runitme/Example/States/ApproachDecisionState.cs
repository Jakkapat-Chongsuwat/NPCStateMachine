using UnityEngine;
using MyGame.StateMachineFramework;

namespace MyGame.NPC
{
    public class ApproachDecisionState<TContext> : BaseState<TContext> where TContext : INpcContext
    {
        public ApproachDecisionState(StateMachine<TContext> parentSM) : base(parentSM) { }

        public override void OnEnter()
        {
            Debug.Log("NPC: Enter ApproachDecisionState");
        }

        public override void OnUpdate()
        {
            // Check context: behind or front?
            if (Context.IsApproachFromBehind)
            {
                // Switch to SurpriseState
                stateMachine.ChangeState(new SurpriseState<TContext>(stateMachine));
            }
            else
            {
                // Switch directly to GreetingState
                stateMachine.ChangeState(new GreetingState<TContext>(stateMachine));
            }
        }

        public override void OnExit()
        {
            Debug.Log("NPC: Exit ApproachDecisionState");
        }
    }
}
