using UnityEngine;
using UnityEngine.AI;
using Jakkapat.ToppuFSM.Core;

namespace Jakkapat.ToppuFSM.Example
{
    public class PatrolState<TContext> : BaseState<TContext> where TContext : INpcContext
    {
        private float waypointThreshold = 1.0f;
        private float roamRange = 10f;
        private Vector3 currentDestination;

        public PatrolState(StateMachine<TContext> parentSM) : base(parentSM) { }

        public override void OnEnter()
        {
            Debug.Log("NPC: Enter PatrolState");
            currentDestination = GetRandomNavMeshPoint(Context.NpcPosition, roamRange);
            SetAgentDestination(currentDestination);
            Context.animationController?.SetSpeed(0f);
            Context.animationController?.SetMotionSpeed(0f);
        }

        public override void OnUpdate()
        {
            if (Context.navMeshAgent == null) return;

            float agentSpeed = Context.navMeshAgent.velocity.magnitude;
            float speedRatio = (Context.navMeshAgent.speed > 0f)
                ? agentSpeed / Context.navMeshAgent.speed
                : 0f;

            Context.animationController?.SetSpeed(agentSpeed);
            Context.animationController?.SetMotionSpeed(speedRatio);

            float distanceToDest = Vector3.Distance(Context.NpcPosition, currentDestination);
            if (distanceToDest < waypointThreshold)
            {
                currentDestination = GetRandomNavMeshPoint(Context.NpcPosition, roamRange);
                SetAgentDestination(currentDestination);
            }
        }

        public override void OnExit()
        {
            Debug.Log("NPC: Exit PatrolState");
        }

        private void SetAgentDestination(Vector3 destination)
        {
            Context.navMeshAgent?.SetDestination(destination);
        }

        private Vector3 GetRandomNavMeshPoint(Vector3 origin, float range)
        {
            Vector3 randomDirection = Random.insideUnitSphere * range + origin;
            NavMesh.SamplePosition(randomDirection, out var hit, range, NavMesh.AllAreas);
            return hit.position;
        }
    }
}
