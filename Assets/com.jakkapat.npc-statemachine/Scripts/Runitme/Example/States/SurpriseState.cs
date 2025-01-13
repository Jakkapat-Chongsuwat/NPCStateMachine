using UnityEngine;
using MyGame.StateMachineFramework;

namespace MyGame.NPC
{
    public class SurpriseState<TContext> : BaseState<TContext> where TContext : INpcContext
    {
        private float surpriseTimer;

        public SurpriseState(StateMachine<TContext> parentSM) : base(parentSM) { }

        public override void OnEnter()
        {
            Debug.Log("NPC: Enter SurpriseState");
            // Possibly play "surprise" animation
            surpriseTimer = 1.0f; // example duration
        }

        public override void OnUpdate()
        {
            if (surpriseTimer > 0)
            {
                surpriseTimer -= Time.deltaTime;
                return;
            }
            // Once done with surprise animation, go to TurnToPlayer
            stateMachine.ChangeState(new TurnToPlayerState<TContext>(stateMachine));
        }

        public override void OnExit()
        {
            Debug.Log("NPC: Exit SurpriseState");
        }
    }
}
