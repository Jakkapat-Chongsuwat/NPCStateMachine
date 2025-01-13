using UnityEngine;
using Jakkapat.StateMachine.Core;

namespace Jakkapat.StateMachine.Example
{
    [CreateAssetMenu(menuName = "Jakkapat/States/GreetingState", fileName = "GreetingState")]
    public class GreetingState : ScriptableState
    {
        [Header("How long to greet before returning idle?")]
        public float greetingTime = 2f;

        [Header("Next state after greeting? (e.g. Idle)")]
        public StateKey idleKey;

        private float _timer;

        public override StateKey OnEnter(BaseContext context, StateKey fromKey)
        {
            _timer = 0f;
            if (context is NPCContext npc)
            {
                npc.HasGreeted = true;
                npc.PlayGreetingAnimation();
            }
            return this.key;
        }

        public override StateKey OnUpdate(BaseContext context)
        {
            if (!(context is NPCContext npc)) return this.key;

            _timer += Time.deltaTime;

            if (npc.IsTargetInRange())
            {
                npc.RotateToFaceTarget();
            }

            if (_timer >= greetingTime)
            {
                // Done greeting => Change to idleKey
                if (idleKey != null)
                {
                    ChangeState(idleKey);
                }
            }
            return this.key;
        }
    }
}
