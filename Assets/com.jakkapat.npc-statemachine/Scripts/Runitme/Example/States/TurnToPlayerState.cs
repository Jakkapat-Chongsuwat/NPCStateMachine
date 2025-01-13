using UnityEngine;
using MyGame.StateMachineFramework;

namespace MyGame.NPC
{
    public class TurnToPlayerState<TContext> : BaseState<TContext> where TContext : INpcContext
    {
        public TurnToPlayerState(StateMachine<TContext> parentSM) : base(parentSM) { }

        public override void OnEnter()
        {
            Debug.Log("NPC: Enter TurnToPlayerState");
            // Possibly start a "turn to face" animation, or instantly face
        }

        public override void OnUpdate()
        {
            // Example: Instantly face the player:
            FacePlayerInstantly();

            // Now go to Greeting
            stateMachine.ChangeState(new GreetingState<TContext>(stateMachine));
        }

        private void FacePlayerInstantly()
        {
            Vector3 direction = (Context.PlayerPosition - Context.NpcPosition).normalized;
            // e.g., you might set an NPC transform's forward or rotation here
            // (Pseudo-code):
            // npcTransform.forward = direction;
        }

        public override void OnExit()
        {
            Debug.Log("NPC: Exit TurnToPlayerState");
        }
    }
}
