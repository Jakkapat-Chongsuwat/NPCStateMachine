using UnityEngine;
using Jakkapat.ToppuFSM.Core;
using System.Collections.Generic;

namespace Jakkapat.ToppuFSM.Example
{
    public class PlayerApproachState<TContext> : HierarchicalState<TContext>
        where TContext : INpcContext
    {
        public PlayerApproachState(
            StateMachine<TContext> parentStateMachine,
            IState<TContext> defaultSubState,
            IState<TContext>[] allSubStates,
            List<ITransition<TContext>> subTransitions
        ) : base(parentStateMachine, defaultSubState)
        {
            foreach (var st in allSubStates)
            {
                if (st is BaseState<TContext> baseSt)
                {
                    baseSt.SetParentStateMachine(subStateMachine);
                }
            }

            foreach (var tr in subTransitions)
            {
                subStateMachine.AddTransition(tr);
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("Enter PlayerApproachState (Hierarchical)");
        }

        public override void OnExit()
        {
            Context.animationController?.SetSpeed(0f);
            Context.animationController?.SetMotionSpeed(0f);

            Context.navMeshAgent?.SetDestination(Context.NpcPosition);
            Context.navMeshAgent.isStopped = false;

            base.OnExit();
        }
    }
}
