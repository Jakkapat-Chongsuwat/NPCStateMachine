using UnityEngine;
using UnityEngine.AI;
using Jakkapat.StateMachine.Core;

namespace Jakkapat.StateMachine.Example
{
    [CreateAssetMenu(menuName = "Jakkapat/StateMachine/NPCContext", fileName = "NPCContext")]
    public class NPCContext : BaseContext,
        IContextMachine<NPCContext>,
        IAgentMovement,
        ITargetable,
        IRotatable,
        ICanSurprise,
        IIdleData,
        ICanGreet,
        IApproachable
    {
        [Header("References (when used as a ScriptableObject)")]
        public NavMeshAgent agent;
        public Transform target;

        [Header("Animation")]
        [SerializeField] private AnimationController _animController;

        [Header("NPC Settings")]
        public float approachRange = 3f;
        public float idleDuration = 2f;
        public bool hasApproached;
        public bool hasGreeted;

        // This is the property we can set from code:
        public float TurnSpeedValue { get; set; } = 3f;

        // If you do a code-based machine approach, store it here if you want:
        // For scriptable approach, you might not use it or keep it null.
        [System.NonSerialized]
        private StateMachine<NPCContext> _machineRef;
        public StateMachine<NPCContext> StateMachine => _machineRef;

        public AnimationController AnimController
        {
            get => _animController;
            set => _animController = value;
        }

        /// <summary>
        /// The "factory method" to create a new instance at runtime
        /// (so you can pass references). 
        /// </summary>
        public static NPCContext Create(
            StateMachine<NPCContext> machine,
            NavMeshAgent agentRef,
            AnimationController animDriver,
            Transform self,
            Transform targ
        )
        {
            // create the ScriptableObject instance in memory
            var instance = ScriptableObject.CreateInstance<NPCContext>();
            instance._machineRef = machine;
            instance.agent = agentRef;
            instance._animController = animDriver;
            instance.selfTransform = self;
            instance.target = targ;
            // default approach range, idle, etc. can remain or be changed after creation
            return instance;
        }

        // Implementation of the interface:
        public NavMeshAgent Agent => agent;
        public float CurrentSpeed => (agent != null) ? agent.velocity.magnitude : 0f;
        public Transform Target => target;

        public float ApproachRange
        {
            get => approachRange;
            set => approachRange = value;
        }

        public bool HasApproached
        {
            get => hasApproached;
            set => hasApproached = value;
        }

        public bool HasGreeted
        {
            get => hasGreeted;
            set => hasGreeted = value;
        }

        public float IdleDuration
        {
            get => idleDuration;
            set => idleDuration = value;
        }

        // We'll override "TurnSpeed" from IRotatable:
        public float TurnSpeed => TurnSpeedValue;

        public Transform SelfTransform => selfTransform;

        public void UpdateContext()
        {
            if (!IsTargetInRange())
            {
                hasApproached = false;
                hasGreeted = false;
            }
        }

        public void StopMovement()
        {
            if (agent)
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
            }
        }

        public void SetDestination(Vector3 position)
        {
            if (agent)
            {
                agent.isStopped = false;
                agent.SetDestination(position);
            }
        }

        public bool IsAtDestination(float tolerance = 0.1f)
        {
            if (!agent) return true;
            if (agent.pathPending) return false;
            return agent.remainingDistance <= tolerance;
        }

        public bool IsTargetInRange()
        {
            if (!selfTransform || !target) return false;
            float dist = Vector3.Distance(selfTransform.position, target.position);
            return dist <= approachRange;
        }

        public void RotateToFaceTarget()
        {
            if (!selfTransform || !target) return;
            Vector3 dir = target.position - selfTransform.position;
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.001f)
            {
                var targetRot = Quaternion.LookRotation(dir.normalized, Vector3.up);
                selfTransform.rotation = Quaternion.Slerp(
                    selfTransform.rotation,
                    targetRot,
                    Time.deltaTime * TurnSpeedValue
                );
            }
        }

        public void RotateToTargetFraction(float fraction)
        {
            if (!selfTransform || !target) return;
            Vector3 dir = target.position - selfTransform.position;
            dir.y = 0f;
            if (dir.sqrMagnitude < 0.0001f) return;

            var targetRot = Quaternion.LookRotation(dir.normalized, Vector3.up);
            selfTransform.rotation = Quaternion.Slerp(
                selfTransform.rotation,
                targetRot,
                fraction * Time.deltaTime * TurnSpeedValue
            );
        }

        public bool IsPlayerApproachFromBehind()
        {
            if (!selfTransform || !target) return false;

            Vector3 npcFwd = selfTransform.forward;
            Vector3 toPlayer = target.position - selfTransform.position;
            toPlayer.y = 0f;
            toPlayer.Normalize();

            float dot = Vector3.Dot(npcFwd, toPlayer);
            return (dot < 0f);
        }

        public void PlaySurpriseAnimation()
        {
            _animController?.SetSurprise();
        }

        public void PlayGreetingAnimation()
        {
            _animController?.SetGreeting();
        }

        public void SetSpeed(float speed)
        {
            _animController?.SetSpeed(speed);
        }

        public void SetMotionSpeed(float speed)
        {
            _animController?.SetMotionSpeed(speed);
        }
    }
}
