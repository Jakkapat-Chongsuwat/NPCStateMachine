using UnityEngine;
using Jakkapat.ToppuFSM.Core;

namespace Jakkapat.ToppuFSM.Example
{
    public class GreetingState<TContext> : BaseState<TContext>
        where TContext : INpcContext
    {
        private float greetingDuration = 5.5f;

        public GreetingState(StateMachine<TContext> parentSM) : base(parentSM) { }

        public override void OnEnter()
        {
            base.OnEnter();
            needsExitTime = true;
            Context.GreetingTimer = greetingDuration;
            Context.IsGreetingDone = false;
            Context.animationController?.SetGreeting();

            Context.navMeshAgent?.SetDestination(Context.NpcPosition);
            Context.navMeshAgent.isStopped = true;
        }

        public override void OnUpdate()
        {
            if (Context.GreetingTimer > 0)
            {
                Context.GreetingTimer -= Time.deltaTime;
                if (Context.GreetingTimer <= 0)
                {
                    Context.IsGreetingDone = true;
                    needsExitTime = false;
                }
            }
        }

        public override void OnExit()
        {
            Context.animationController?.ResetGreeting();
            Debug.Log("NPC: Exit GreetingState");
        }
    }
}
