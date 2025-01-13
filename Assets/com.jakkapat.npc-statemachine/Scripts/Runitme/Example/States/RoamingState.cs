using UnityEngine;
using Jakkapat.StateMachine.Core;

namespace Jakkapat.StateMachine.Example
{
    [CreateAssetMenu(menuName = "Jakkapat/States/RoamingState", fileName = "RoamingState")]
    public class RoamingState : ScriptableState
    {
        [Header("Roam radius")]
        public float roamRadius = 10f;

        public StateKey playerApproachKey; // assign in Inspector
        public StateKey idleKey;           // assign in Inspector

        public override StateKey OnEnter(BaseContext context, StateKey fromKey)
        {
            if (context is NpcContext npc && npc.Agent)
            {
                npc.Agent.isStopped = false;
                Vector3 roamTarget = GetRandomRoamPosition(npc.selfTransform.position, roamRadius);
                npc.SetDestination(roamTarget);
            }
            return this.key;
        }

        public override StateKey OnUpdate(BaseContext context)
        {
            if (!(context is NpcContext npc)) return this.key;

            // if player out-of-range => npc.HasApproached = false
            if (!npc.IsTargetInRange())
            {
                npc.HasApproached = false;
            }
            else
            {
                if (!npc.HasApproached)
                {
                    // we want to approach
                    npc.StopMovement();
                    return playerApproachKey;
                }
                else
                {
                    // keep turning to face
                    npc.RotateToFaceTarget();
                }
            }

            // check if agent done
            if (npc.Agent && !npc.Agent.pathPending)
            {
                if (npc.Agent.remainingDistance <= npc.Agent.stoppingDistance + 0.01f)
                {
                    // go idle
                    return idleKey;
                }
                else
                {
                    float speed = npc.Agent.velocity.magnitude;
                    npc.SetSpeed(speed);
                    npc.SetMotionSpeed(speed / npc.Agent.speed);
                }
            }

            return this.key;
        }

        Vector3 GetRandomRoamPosition(Vector3 origin, float radius)
        {
            Vector2 rand = Random.insideUnitCircle * radius;
            return new Vector3(origin.x + rand.x, origin.y, origin.z + rand.y);
        }
    }
}
