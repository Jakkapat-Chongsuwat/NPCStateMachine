using UnityEngine;
using MyGame.StateMachineFramework;

namespace MyGame.NPC
{
    public class GreetingState<TContext> : BaseState<TContext> where TContext : INpcContext
    {
        private float greetingDuration = 1.5f;

        public GreetingState(StateMachine<TContext> parentSM) : base(parentSM) { }

        public override void OnEnter()
        {
            Debug.Log("NPC: Enter GreetingState");
            Context.GreetingTimer = greetingDuration;
            // e.g., play greet animation
        }

        public override void OnUpdate()
        {
            // Decrease timer
            if (Context.GreetingTimer > 0)
            {
                Context.GreetingTimer -= Time.deltaTime;
                return;
            }
            // Time is up: go to Idle
            stateMachine.ChangeState(new IdleState<TContext>(stateMachine));
        }

        public override void OnExit()
        {
            Debug.Log("NPC: Exit GreetingState");
        }
    }
}
