using UnityEngine;
using MyGame.StateMachineFramework;

namespace MyGame.NPC
{
    public class IdleState<TContext> : BaseState<TContext> where TContext : INpcContext
    {
        public IdleState(StateMachine<TContext> parentSM) : base(parentSM) { }

        public override void OnEnter()
        {
            Debug.Log("NPC: Enter IdleState - Always face the player");
            // e.g., play idle animation
        }

        public override void OnUpdate()
        {
            // Keep facing the player
            FacePlayer();
        }

        private void FacePlayer()
        {
            Vector3 direction = (Context.PlayerPosition - Context.NpcPosition).normalized;
            // e.g. npcTransform.forward = direction;
        }

        public override void OnExit()
        {
            Debug.Log("NPC: Exit IdleState");
        }
    }
}
