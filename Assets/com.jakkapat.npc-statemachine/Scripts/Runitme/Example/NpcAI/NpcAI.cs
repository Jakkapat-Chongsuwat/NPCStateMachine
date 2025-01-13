using UnityEngine;
using MyGame.StateMachineFramework;

namespace MyGame.NPC
{
    public class NpcAI : MonoBehaviour
    {
        private NpcContext npcContext;
        private StateMachine<NpcContext> mainStateMachine;

        public Transform playerTransform;
        public float behindDotThreshold = 0.5f;
        public float approachDistance = 3.0f;

        void Start()
        {
            playerTransform = GameObject.FindWithTag("Player").transform;

            // 1. Create context
            npcContext = new NpcContext();
            npcContext.animationController = GetComponent<AnimationController>();

            // 2. Create initial (start) state - we can pass null for now
            //    then later we call 'SetParentStateMachine(...)'
            var patrolState = new PatrolState<NpcContext>(null);

            // 3. Create the main state machine with the context
            mainStateMachine = new StateMachine<NpcContext>(npcContext, patrolState);

            // Fix the parent's reference:
            patrolState.SetParentStateMachine(mainStateMachine);

            // 3.1. We need a sub-state for PlayerApproachState, e.g. ApproachDecisionState
            var approachDecision = new ApproachDecisionState<NpcContext>(null);
            // We'll set its parent SM later as well

            // 4. Create PlayerApproachState with the default sub-state
            //    Because your HierarchicalState wants (parentSM, defaultSubState)
            var playerApproachState = new PlayerApproachState<NpcContext>(
                mainStateMachine,
                approachDecision
            );

            // Now fix approachDecision to reference the subStateMachine from playerApproachState
            // But note that the subStateMachine is inside PlayerApproachState. 
            // You can expose a public property or method to retrieve or set it. For example:
            //  (approachDecision as BaseState<NpcContext>).SetParentStateMachine(
            //      playerApproachState.SubStateMachine
            //  );

            // 5. Add transitions (outside of states)
            mainStateMachine.AddTransition(new Transition<NpcContext>(
                fromState: patrolState,
                toState: playerApproachState,
                condition: (ctx) => ctx.IsPlayerApproaching
            ));

            // 6. We’re done (for now). The sub-states handle behind vs. front logic themselves.
        }

        void Update()
        {
            // Update our context each frame
            npcContext.NpcPosition = transform.position;
            npcContext.PlayerPosition = playerTransform.position;

            // Decide if player is approaching
            float distance = Vector3.Distance(npcContext.NpcPosition, npcContext.PlayerPosition);
            npcContext.IsPlayerApproaching = (distance < approachDistance);

            // Decide if approach is from behind
            Vector3 toPlayer = (playerTransform.position - transform.position).normalized;
            float dot = Vector3.Dot(transform.forward, toPlayer);
            npcContext.IsApproachFromBehind = (dot < behindDotThreshold);

            // Tick the state machine
            mainStateMachine.Update();
        }
    }
}
