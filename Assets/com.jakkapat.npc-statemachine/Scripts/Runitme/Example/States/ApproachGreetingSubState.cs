using UnityEngine;
using Jakkapat.StateMachine.Core;

namespace Jakkapat.StateMachine.Example
{
    [CreateAssetMenu(menuName = "Jakkapat/States/ApproachGreeting", fileName = "ApproachGreetingSubState")]
    public class ApproachGreetingSubState : ScriptableState
    {
        [Tooltip("How long we greet before going idle?")]
        public float greetingDuration = 2.0f;

        public StateKey approachIdleKey;

        private float _timer;

        public override StateKey OnEnter(BaseContext context, StateKey fromKey)
        {
            _timer = 0f;
            if (context is NPCContext npc)
            {
                npc.RotateToFaceTarget();
                npc.PlayGreetingAnimation();
            }
            return this.key;
        }

        public override StateKey OnUpdate(BaseContext context)
        {
            _timer += Time.deltaTime;
            if (_timer >= greetingDuration)
            {
                // go approach idle
                return approachIdleKey;
            }
            return this.key;
        }
    }
}
