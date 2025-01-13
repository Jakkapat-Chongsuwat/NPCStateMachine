using UnityEngine;
using Jakkapat.StateMachine.Core;
using System;

namespace Jakkapat.StateMachine.Example
{
    public class IdleState<TContext, TStateID>
           : BaseState<TContext, TStateID>
           where TContext : class,
                           ITargetable,
                           IApproachable,
                           IRotatable,
                           IAgentMovement,
                           IContextMachine<TContext, TStateID>
           where TStateID : struct, Enum
    {
        private readonly TStateID _id;

        public float IdleDuration { get; set; }
        private float _timer = 0f;

        public IdleState(TStateID id, float idleDuration = 2f)
        {
            _id = id;
            IdleDuration = idleDuration;
        }

        public override TStateID ID => _id;

        public override void EnterState(TContext context, TStateID fromState)
        {
            _timer = 0f;

            // context is definitely IAgentMovement => call Stop
            context.StopMovement();

            Debug.Log($"[IdleState] Enter with ID={_id}, from={fromState}");
        }

        public override void UpdateState(TContext context)
        {
            _timer += Time.deltaTime;

            // context is ITargetable => we can check range
            bool inRange = context.IsTargetInRange();
            if (inRange)
            {
                // context is IApproachable => check HasApproached
                if (!context.HasApproached)
                {
                    // Not approached => do approach logic or do nothing
                }
                else
                {
                    // context is IRotatable => face target
                    context.RotateToFaceTarget();
                }
            }
            else
            {
                // Player left range => reset approach
                context.HasApproached = false;
            }

            if (_timer >= IdleDuration)
            {
                Debug.Log($"[IdleState] Idle done => changing to default state ID (just for example).");
                // context is IContextMachine => we can do a state transition
                context.StateMachine.ChangeState(default);
            }
        }

        public override void ExitState(TContext context, TStateID toState)
        {
            Debug.Log($"[IdleState] Exit from ID={_id} to {toState}");
        }
    }
}
