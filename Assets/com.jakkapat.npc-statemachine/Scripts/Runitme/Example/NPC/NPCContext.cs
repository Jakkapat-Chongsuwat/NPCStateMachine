namespace Jakkapat.StateMachine.Example
{
    using Jakkapat.StateMachine.Core;
    using UnityEngine;
    using UnityEngine.AI;

    public class NPCContext :
        IContextMachine<NPCContext, NPCStates>,
        IAgentMovement,
        ITargetable,
        IRotatable,
        ICanSurprise,
        ICanGreet,
        IApproachable
    {
        // The machine we store
        public StateMachine<NPCContext, NPCStates> StateMachine { get; private set; }

        // Public references
        public NavMeshAgent Agent { get; set; }
        public Transform SelfTransform { get; set; }
        public Transform Target { get; set; }

        // Additional ref if needed
        public AnimationController animationController { get; set; }

        // Movement / approach settings
        public float TurnSpeed { get; set; } = 3f;
        public float ApproachRange { get; set; } = 3f;

        // Approach flags
        public bool HasApproached { get; set; }
        public bool HasGreeted { get; set; }

        public float CurrentSpeed => 0f;

        // Constructor: we create a new machine
        public NPCContext()
        {
            StateMachine = new StateMachine<NPCContext, NPCStates>(this);
        }

        // IAgentMovement
        public void StopMovement()
        {
            if (Agent)
            {
                Agent.isStopped = true;
                Agent.velocity = Vector3.zero;
            }
        }

        public bool IsAtDestination(float tolerance = 0.1f)
        {
            if (!Agent) return true;
            if (Agent.pathPending) return false;
            return Agent.remainingDistance <= tolerance;
        }

        // ITargetable
        public bool IsTargetInRange()
        {
            if (!SelfTransform || !Target) return false;
            float dist = Vector3.Distance(SelfTransform.position, Target.position);
            return dist <= ApproachRange;
        }

        // IRotatable
        public void RotateToFaceTarget()
        {
            if (!SelfTransform || !Target) return;
            Vector3 dir = Target.position - SelfTransform.position;
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(dir.normalized, Vector3.up);
                SelfTransform.rotation = Quaternion.Slerp(
                    SelfTransform.rotation,
                    targetRot,
                    Time.deltaTime * TurnSpeed
                );
            }
        }

        public void RotateToTargetFraction(float fraction)
        {
            if (!SelfTransform || !Target) return;
            Vector3 dir = Target.position - SelfTransform.position;
            dir.y = 0f;
            if (dir.sqrMagnitude < 0.0001f) return;
            Quaternion targetRot = Quaternion.LookRotation(dir.normalized, Vector3.up);
            SelfTransform.rotation = Quaternion.Slerp(
                SelfTransform.rotation,
                targetRot,
                fraction * Time.deltaTime * TurnSpeed
            );
        }

        // IApproachable
        public bool IsPlayerApproachFromBehind()
        {
            if (!SelfTransform || !Target) return false;
            Vector3 forward = SelfTransform.forward;
            Vector3 toPlayer = Target.position - SelfTransform.position;
            toPlayer.y = 0f;
            toPlayer.Normalize();
            float dot = Vector3.Dot(forward, toPlayer);
            return dot < 0f;
        }

        // ICanSurprise, ICanGreet
        public void PlaySurpriseAnimation()
        {
            animationController?.SetSurprise();
        }

        public void PlayGreetingAnimation()
        {
            animationController?.SetGreeting();
        }

        // Optional: update logic each frame if you wish
        public void UpdateContext()
        {
            // If out of range => reset approach flags
            if (!IsTargetInRange())
            {
                HasApproached = false;
                HasGreeted = false;
            }
        }

        public void SetDestination(Vector3 position)
        {
            if (!Agent) return;
            Agent.isStopped = false;
            Agent.SetDestination(position);
        }
    }
}