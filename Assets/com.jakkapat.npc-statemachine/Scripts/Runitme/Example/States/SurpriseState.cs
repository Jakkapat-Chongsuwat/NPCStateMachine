using UnityEngine;
using System;
using Jakkapat.StateMachine.Example;

namespace Jakkapat.StateMachine.Core
{
    /// <summary>
    /// A generic SurpriseState that:
    /// 1. Requires the context to implement IApproachable, IRotatable, ITargetable, ICanSurprise,
    ///    and IContextMachine for transitions.
    /// 2. On Enter, sets HasApproached to true and plays a surprise animation.
    /// 3. On Update, does a partial rotation for a certain time, then fully faces the target,
    ///    and eventually transitions to a 'greeting' state ID.
    /// </summary>
    /// <typeparam name="TContext">
    /// A class that implements 
    ///   IApproachable, IRotatable, ITargetable, ICanSurprise, 
    ///   and IContextMachine (so we can transition states).
    /// </typeparam>
    /// <typeparam name="TStateID">
    /// An enum (or struct with 'Enum' constraint) representing state IDs.
    /// </typeparam>
    public class SurpriseState<TContext, TStateID>
        : BaseState<TContext, TStateID>
        where TContext : class,
                        IApproachable,
                        IRotatable,
                        ITargetable,
                        ICanSurprise,
                        ICanGreet, /* if you also want to call context.PlayGreetingAnimation() */
                        IContextMachine<TContext, TStateID>
        where TStateID : struct, Enum
    {
        private readonly TStateID _id;         // The ID of this SurpriseState
        private readonly TStateID _greetingID; // The state ID to go to after surprise finishes

        // How long we remain in "surprise" state total
        public float SurpriseDuration { get; set; }

        // How many seconds we do partial rotation (50%) before fully facing the target
        public float PartialRotationTime { get; set; }

        private float _timer = 0f;

        public SurpriseState(
            TStateID stateID,
            TStateID greetingID,
            float surpriseDuration = 1.5f,
            float partialRotationTime = 1.0f)
        {
            _id = stateID;
            _greetingID = greetingID;
            SurpriseDuration = surpriseDuration;
            PartialRotationTime = partialRotationTime;
        }

        public override TStateID ID => _id;

        public override void EnterState(TContext context, TStateID fromState)
        {
            _timer = 0f;

            // Mark approached
            context.HasApproached = true;

            // Start the surprise animation
            context.PlaySurpriseAnimation();
        }

        public override void UpdateState(TContext context)
        {
            _timer += Time.deltaTime;

            // If the NPC can see the player or is in range
            if (context.IsTargetInRange())
            {
                if (_timer < PartialRotationTime)
                {
                    // Only partially rotate (e.g. 50% fraction)
                    context.RotateToTargetFraction(0.5f);
                }
                else
                {
                    // Now fully face the player
                    context.RotateToFaceTarget();
                }
            }

            // Once we've spent enough time in surprise, transition to greeting
            if (_timer >= SurpriseDuration)
            {
                // Optionally fully face before greeting
                context.RotateToFaceTarget();

                // If your context also implements ICanGreet, you can call:
                context.PlayGreetingAnimation();

                // Then do the actual state transition
                context.StateMachine.ChangeState(_greetingID);
            }
        }

        public override void ExitState(TContext context, TStateID toState)
        {
            // Optional: any cleanup if needed
        }
    }
}
