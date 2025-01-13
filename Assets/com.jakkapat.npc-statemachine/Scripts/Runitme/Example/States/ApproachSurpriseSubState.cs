using UnityEngine;
using Jakkapat.StateMachine.Core;

namespace Jakkapat.StateMachine.Example
{
    [CreateAssetMenu(menuName = "Jakkapat/States/ApproachSurprise", fileName = "ApproachSurpriseSubState")]
    public class ApproachSurpriseSubState : ScriptableState
    {
        [Tooltip("How long we remain in surprise before greeting?")]
        public float surpriseDuration = 1.0f;

        public StateKey approachGreetingKey;

        private float _timer;

        public override StateKey OnEnter(BaseContext context, StateKey fromKey)
        {
            _timer = 0f;
            if (context is NPCContext npc)
            {
                npc.HasApproached = true;
                npc.RotateToTargetFraction(0.5f);
                npc.PlaySurpriseAnimation();
            }
            return this.key;
        }

        public override StateKey OnUpdate(BaseContext context)
        {
            _timer += Time.deltaTime;

            if (_timer >= surpriseDuration)
            {
                // go greeting
                return approachGreetingKey;
            }
            return this.key;
        }
    }
}
