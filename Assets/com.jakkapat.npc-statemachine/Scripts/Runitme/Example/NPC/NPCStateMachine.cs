using UnityEngine;
using UnityEngine.AI;
using Jakkapat.StateMachine.Core;

namespace Jakkapat.StateMachine.Example
{
    [RequireComponent(typeof(CharacterController))]
    public class NpcStateMachine : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Transform _target;

        [Header("Settings")]
        [SerializeField] private float _approachRange = 3f;
        [SerializeField] private float _turnSpeed = 3f;

        private NpcContext _context;
        private StateMachine<NpcContext, NpcStates> _machine;

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
            // 1) Create the context
            _context = new NpcContext
            {
                Agent = _agent,
                SelfTransform = transform,
                Target = _target,
                ApproachRange = _approachRange,
                TurnSpeed = _turnSpeed
            };

            // 2) Create the machine
            _machine = new StateMachine<NpcContext, NpcStates>();

            // 3) Set the context into the machine
            _machine.Context = _context;

            // 4) Also let the context store the machine internally if states want to call
            _context.SetStateMachine(_machine);

            // 5) Add states
            // e.g. some generic state "PlayerApproachState<TContext, TStateID>" 
            // that implements IState<NpcContext, NpcStates> with ID = NpcStates.PlayerApproach
            _machine.AddState(new PlayerApproachState<NpcContext, NpcStates>(NpcStates.PlayerApproach));

            // 6) Start the machine
            _machine.ChangeState(NpcStates.PlayerApproach);
        }

        private void Update()
        {
            // Let the context do its per-frame logic
            _context.UpdateContext();

            // Update the machine
            _machine.Update();
        }
    }
}
