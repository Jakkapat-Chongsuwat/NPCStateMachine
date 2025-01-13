using UnityEngine;
using UnityEngine.AI;
using Jakkapat.StateMachine.Core;

namespace Jakkapat.StateMachine.Example
{
    [RequireComponent(typeof(CharacterController))]
    public class NPCStateMachine : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Transform _target;

        [Header("Settings")]
        [SerializeField] private float _approachRange = 3f;
        [SerializeField] private float _turnSpeed = 3f;

        // We'll store the context and the machine
        private NPCContext _context;
        private StateMachine<NPCContext> _machine;

        private void Awake()
        {
            if (!_agent) _agent = GetComponent<NavMeshAgent>();
            if (!_target)
            {
                var playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj) _target = playerObj.transform;
            }
        }

        private void Start()
        {
            // Create the context
            _context = new NPCContext
            {
                Agent = _agent,
                SelfTransform = transform,
                Target = _target,
                ApproachRange = _approachRange,
                TurnSpeed = _turnSpeed
            };

            // The context has a .StateMachine property,
            // but let's store a reference to it:
            _machine = _context.StateMachine;

            // Add top-level states like Idle, Roaming, etc. 
            // For demonstration, let's just add PlayerApproach:
            _machine.AddState(new PlayerApproachState<NPCContext>());

            // Start in PlayerApproach or something else:
            _machine.ChangeState(Core.StateIDs.PlayerApproach);
        }

        private void Update()
        {
            // Optionally let context do per-frame logic
            _context.UpdateContext();

            // Let the state machine update
            _machine.Update();
        }
    }
}
