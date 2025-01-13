using UnityEngine;
using Jakkapat.StateMachine.Core;

namespace Jakkapat.StateMachine.Example
{
    [CreateAssetMenu(menuName = "Jakkapat/States/SurpriseState", fileName = "SurpriseState")]
    public class SurpriseState : ScriptableState
    {
        [Header("Duration of surprise (seconds)")]
        public float surpriseDuration = 1.5f;

        [Header("Time to do partial rotation (seconds)")]
        public float partialRotationTime = 1.0f;

        [Header("State to go after surprise finishes (e.g. Greeting)")]
        public StateKey greetingKey;

        private float _timer;

        public override StateKey OnEnter(BaseContext context, StateKey fromKey)
        {
            _timer = 0f;

            if (context is NPCContext npc)
            {
                npc.HasApproached = true;
                npc.PlaySurpriseAnimation();
            }

            return this.key;
        }

        public override StateKey OnUpdate(BaseContext context)
        {
            if (!(context is NPCContext npc)) return this.key;

            _timer += Time.deltaTime;

            if (npc.IsTargetInRange())
            {
                if (_timer < partialRotationTime)
                {
                    npc.RotateToTargetFraction(0.5f);
                }
                else
                {
                    npc.RotateToFaceTarget();
                }
            }

            if (_timer >= surpriseDuration)
            {
                // Transition to greetingKey
                if (greetingKey != null)
                {
                    ChangeState(greetingKey);
                }
            }

            return this.key;
        }
    }
}
