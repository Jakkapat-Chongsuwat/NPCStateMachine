using UnityEngine;
using Jakkapat.StateMachine.Core;

namespace Jakkapat.StateMachine.Example
{
    [CreateAssetMenu(menuName = "Jakkapat/States/PlayerApproach", fileName = "PlayerApproachState")]
    public class PlayerApproachState : NestableScriptableState
    {
        [Header("StateKey to go if out-of-range (e.g. 'Roaming')")]
        public StateKey roamingKey;

        public override StateKey OnEnter(BaseContext baseCtx, StateKey fromKey)
        {
            // 1) Call base to init subMachine
            var result = base.OnEnter(baseCtx, fromKey);

            // 2) If we have an NPCContext, stop movement, set hasApproached
            if (baseCtx is NPCContext npc)
            {
                npc.StopMovement();
                npc.HasApproached = true;
            }
            return result;
        }

        public override StateKey OnUpdate(BaseContext baseCtx)
        {
            // 1) Let sub-machine run
            var result = base.OnUpdate(baseCtx);

            // 2) If the user left range => go to roaming
            if (baseCtx is NPCContext npc)
            {
                if (!npc.IsTargetInRange())
                {
                    npc.HasApproached = false;
                    return roamingKey;
                }
            }

            // stay in approach
            return this.key;
        }
    }
}
