using UnityEngine;
using Jakkapat.ToppuFSM.Core;

namespace Jakkapat.ToppuFSM.Example
{
    public class SurpriseState<TContext> : BaseState<TContext>
        where TContext : INpcContext
    {
        public SurpriseState(StateMachine<TContext> parentSM) : base(parentSM) { }

        public override void OnEnter()
        {
            base.OnEnter();
            needsExitTime = true;

            if (Context.navMeshAgent != null)
            {
                Context.navMeshAgent.isStopped = true;
            }

            Context.SurpriseTimer = 3.0f;
            Context.SurpriseDone = false;
            Context.animationController?.SetSurprise();
        }

        public override void OnUpdate()
        {
            if (Context.SurpriseTimer > 0)
            {
                Context.SurpriseTimer -= Time.deltaTime;
                if (Context.SurpriseTimer <= 0)
                {
                    Context.SurpriseDone = true;
                    needsExitTime = false;
                }
            }
        }

        public override void OnExit()
        {
            Context.animationController?.SetStopSurprise();
            Debug.Log("NPC: Exit SurpriseState");
        }
    }
}
