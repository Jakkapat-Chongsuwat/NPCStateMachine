using UnityEngine;
using Jakkapat.StateMachine.Core;

namespace Jakkapat.StateMachine.Example
{
    [CreateAssetMenu(menuName = "Jakkapat/States/IdleState", fileName = "IdleState")]
    public class IdleState : ScriptableState
    {
        public float idleTime = 2f;
        public StateKey roamingKey;

        private float _timer;


        public override StateKey OnEnter(BaseContext context, StateKey fromKey)
        {
            _timer = 0f;
            if (context is NPCContext npc)
            {
                npc.StopMovement();
            }
            return this.key;
        }

        public override StateKey OnUpdate(BaseContext context)
        {
            _timer += Time.deltaTime;
            if (!(context is NPCContext npc)) return this.key;

            if (npc.IsTargetInRange())
            {
                if (!npc.HasApproached)
                {
                    // maybe do nothing, or approach logic, etc.
                }
                else
                {
                    npc.RotateToFaceTarget();
                }
            }
            else
            {
                npc.HasApproached = false;
            }

            if (_timer >= npc.IdleDuration)
            {
                // Transition to Roaming
                return roamingKey;
            }
            return this.key;
        }
    }
}
