using UnityEngine;
using Jakkapat.StateMachine.Core;

namespace Jakkapat.StateMachine.Example
{
    [CreateAssetMenu(menuName = "Jakkapat/States/ApproachInitial", fileName = "ApproachInitialSubState")]
    public class ApproachInitialSubState : ScriptableState
    {
        public StateKey approachSurpriseKey;
        public StateKey approachGreetingKey;

        public override StateKey OnEnter(BaseContext context, StateKey fromKey)
        {
            // do nothing
            return this.key;
        }

        public override StateKey OnUpdate(BaseContext context)
        {
            if (context is NPCContext npc)
            {
                if (npc.IsPlayerApproachFromBehind())
                {
                    npc.HasApproached = true;
                    // go surprise
                    return approachSurpriseKey;
                }
                else
                {
                    npc.HasApproached = true;
                    npc.PlayGreetingAnimation();
                    // go greeting
                    return approachGreetingKey;
                }
            }
            return this.key;
        }
    }
}
