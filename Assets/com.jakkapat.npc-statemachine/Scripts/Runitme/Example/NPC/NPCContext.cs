using Jakkapat.StateMachine.Core;
using UnityEngine;
using UnityEngine.AI;

namespace Jakkapat.StateMachine.Example
{
    /// <summary>
    /// Inherits from BaseContext (which is now a normal class),
    /// plus implements the various interfaces (IAgentMovement, etc.).
    /// </summary>
    public class NpcContext : BaseContext,
        IContextMachine<NpcContext, NpcStates>,
        IAgentMovement,
        ITargetable,
        IRotatable,
        ICanSurprise,
        ICanGreet,
        IApproachable
    {
        // 1) The strongly-typed state machine reference:
        private StateMachine<NpcContext, NpcStates> _machine;
        public StateMachine<NpcContext, NpcStates> StateMachine => _machine;

        // Optionally let external code set the machine if needed:
        public void SetStateMachine(StateMachine<NpcContext, NpcStates> machine)
        {
            _machine = machine;
        }

        // 2) References we want to store:
        public NavMeshAgent Agent { get; set; }
        public Transform SelfTransform { get; set; }
        public Transform Target { get; set; }
        public AnimationController animationController { get; set; }

        // 3) Additional config:
        public float TurnSpeed { get; set; } = 3f;
        public float ApproachRange { get; set; } = 3f;

        // 4) Approach flags:
        public bool HasApproached { get; set; }
        public bool HasGreeted { get; set; }

        // 5) For demonstration, let's assume this is your "current speed" logic:
        public float CurrentSpeed => Agent ? Agent.velocity.magnitude : 0f;

        // -------------------------------------------------------------
        //  IAgentMovement
        // -------------------------------------------------------------
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

        public void SetDestination(Vector3 position)
        {
            if (!Agent) return;
            Agent.isStopped = false;
            Agent.SetDestination(position);
        }

        // -------------------------------------------------------------
        //  ITargetable
        // -------------------------------------------------------------
        public bool IsTargetInRange()
        {
            if (!SelfTransform || !Target) return false;
            float dist = Vector3.Distance(SelfTransform.position, Target.position);
            return dist <= ApproachRange;
        }

        // -------------------------------------------------------------
        //  IRotatable
        // -------------------------------------------------------------
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

        // -------------------------------------------------------------
        //  IApproachable
        // -------------------------------------------------------------
        public bool IsPlayerApproachFromBehind()
        {
            if (!SelfTransform || !Target) return false;
            Vector3 forward = SelfTransform.forward;
            Vector3 toPlayer = Target.position - SelfTransform.position;
            toPlayer.y = 0f;
            toPlayer.Normalize();
            float dot = Vector3.Dot(forward, toPlayer);
            return (dot < 0f);
        }

        // -------------------------------------------------------------
        //  ICanSurprise, ICanGreet
        // -------------------------------------------------------------
        public void PlaySurpriseAnimation()
        {
            animationController?.SetSurprise();
        }

        public void PlayGreetingAnimation()
        {
            animationController?.SetGreeting();
        }

        // -------------------------------------------------------------
        //  Optional: per-frame logic
        // -------------------------------------------------------------
        public void UpdateContext()
        {
            // e.g. if out of range => reset approach flags
            if (!IsTargetInRange())
            {
                HasApproached = false;
                HasGreeted = false;
            }
        }
    }
}
