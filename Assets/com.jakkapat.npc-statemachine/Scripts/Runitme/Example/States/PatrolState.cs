using UnityEngine;
using MyGame.StateMachineFramework;

namespace MyGame.NPC
{
    public class PatrolState<TContext> : BaseState<TContext> where TContext : INpcContext
    {
        public PatrolState(StateMachine<TContext> parentSM) : base(parentSM) { }

        public override void OnEnter()
        {
            Debug.Log("NPC: Enter Patrol");
            // e.g., play patrol animation, reset path
        }

        public override void OnUpdate()
        {
            // Some patrol logic, e.g. move between waypoints
            // Example: If we detect the player is approaching, set IsPlayerApproaching = true
            // That will trigger the external transition to PlayerApproachState.
        }

        public override void OnExit()
        {
            Debug.Log("NPC: Exit Patrol");
        }
    }
}
