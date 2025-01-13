using UnityEngine;
using UnityEngine.AI;
using Jakkapat.StateMachine.Core;

namespace Jakkapat.StateMachine.Example
{
    [RequireComponent(typeof(CharacterController))]
    public class NPCStateMachine : MonoBehaviour
    {
        [Header("Scriptable Approach")]
        [SerializeField] private ScriptableStateMachine scriptableMachine;

        [Header("References")]
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private AnimationController _animController;
        [SerializeField] private Transform _target;

        [Header("NPC Settings")]
        [SerializeField] private float _idleDuration = 2f;
        [SerializeField] private float _turnSpeed = 3f;
        [SerializeField] private float _approachRange = 3f;

        private NPCContext _context;

        void Awake()
        {
            if (!_agent) _agent = GetComponent<NavMeshAgent>();
            if (!_target)
            {
                var obj = GameObject.FindGameObjectWithTag("Player");
                if (obj) _target = obj.transform;
            }
        }

        void Start()
        {
            // If you want a dynamic instance each time:
            _context = ScriptableObject.CreateInstance<NPCContext>();
            _context.agent = _agent;
            _context.target = _target;
            _context.AnimController = _animController;
            _context.selfTransform = this.transform;

            // apply your NPC settings
            _context.turnSpeed = _turnSpeed;
            _context.approachRange = _approachRange;
            _context.idleDuration = _idleDuration;

            // 3) Initialize the scriptable machine
            if (scriptableMachine)
            {
                scriptableMachine.Init(_context);
            }
            else
            {
                Debug.LogWarning("No ScriptableStateMachine assigned!");
            }
        }

        void Update()
        {
            // Let context do per-frame logic if you want
            _context.UpdateContext();

            // Update the scriptable machine
            if (scriptableMachine)
            {
                scriptableMachine.Tick();
            }
        }
    }
}
