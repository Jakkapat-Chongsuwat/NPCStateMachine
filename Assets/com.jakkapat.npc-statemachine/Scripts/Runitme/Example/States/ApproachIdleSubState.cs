using UnityEngine;
using Jakkapat.StateMachine.Core;

namespace Jakkapat.StateMachine.Example
{
    [CreateAssetMenu(menuName = "Jakkapat/States/ApproachIdle", fileName = "ApproachIdleSubState")]
    public class ApproachIdleSubState : ScriptableState
    {
        public override StateKey OnUpdate(BaseContext context)
        {
            if (context is NPCContext npc)
            {
                if (npc.IsTargetInRange())
                {
                    npc.RotateToFaceTarget();
                }
            }
            // remain in approach idle
            return this.key;
        }
    }
}
