using UnityEngine;
using Jakkapat.ToppuFSM.Core;

namespace Jakkapat.ToppuFSM.Example
{
    public class IdleState<TContext> : BaseState<TContext> where TContext : INpcContext
    {
        public IdleState(StateMachine<TContext> parentSM) : base(parentSM) { }

        public override void OnEnter()
        {
            Context.animationController?.SetSpeed(0f);
            Context.animationController?.SetMotionSpeed(0f);
            Debug.Log("NPC: Enter IdleState");
        }

        public override void OnUpdate()
        {
            Vector3 dir = (Context.PlayerPosition - Context.NpcPosition).normalized;
            // e.g. transform.forward = dir;
        }

        public override void OnExit()
        {
            Debug.Log("NPC: Exit IdleState");
        }
    }
}
