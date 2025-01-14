using UnityEngine;
using Jakkapat.ToppuFSM.Core;

namespace Jakkapat.ToppuFSM.Example
{
    public class ApproachDecisionState<TContext> : BaseState<TContext>
        where TContext : INpcContext
    {
        public ApproachDecisionState(StateMachine<TContext> parentSM) : base(parentSM) { }

        public override void OnEnter()
        {
            // We just begin the decision
            Context.IsApproachDecisionComplete = false;
            Debug.Log("NPC: Enter ApproachDecisionState");
        }

        public override void OnUpdate()
        {
            // We decide behind or front
            bool behind = Context.IsApproachFromBehind;

            // Record that we made our decision
            Context.ApproachFromBehind = behind;
            Context.IsApproachDecisionComplete = true;
        }

        public override void OnExit()
        {
            Debug.Log("NPC: Exit ApproachDecisionState");
        }
    }
}
