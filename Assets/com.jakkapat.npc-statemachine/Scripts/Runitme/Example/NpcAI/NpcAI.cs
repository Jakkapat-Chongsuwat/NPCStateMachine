using UnityEngine;
using UnityEngine.AI;
using Jakkapat.ToppuFSM.Core;
using System.Collections.Generic;

namespace Jakkapat.ToppuFSM.Example
{
    public class NpcAI : MonoBehaviour
    {
        private NpcContext npcContext;
        private StateMachine<NpcContext> fsm;

        public Transform playerTransform;
        public float behindDotThreshold = 0.5f;
        public float approachDistance = 3.0f;

        void Start()
        {
            playerTransform = GameObject.FindWithTag("Player").transform;

            npcContext = new NpcContext
            {
                animationController = GetComponent<AnimationController>(),
                navMeshAgent = GetComponent<NavMeshAgent>(),

                NpcTransform = this.transform
            };

            fsm = new StateMachine<NpcContext>(npcContext, null);

            var patrolState = new PatrolState<NpcContext>(fsm);

            // sub-states
            var approachDecision = new ApproachDecisionState<NpcContext>(null);
            var surpriseState = new SurpriseState<NpcContext>(null);
            var turnToPlayer = new TurnToPlayerState<NpcContext>(null);
            var greetingState = new GreetingState<NpcContext>(null);
            var idleState = new IdleState<NpcContext>(null);

            var subTransitions = new List<ITransition<NpcContext>>
            {
                new Transition<NpcContext>(approachDecision, surpriseState,
                    ctx => ctx.IsApproachDecisionComplete && ctx.ApproachFromBehind),
                new Transition<NpcContext>(approachDecision, greetingState,
                    ctx => ctx.IsApproachDecisionComplete && !ctx.ApproachFromBehind),
                new Transition<NpcContext>(surpriseState, turnToPlayer,
                    ctx => ctx.SurpriseDone),
                new Transition<NpcContext>(turnToPlayer, greetingState,
                    ctx => ctx.HasFacedPlayer),
                new Transition<NpcContext>(greetingState, idleState,
                    ctx => ctx.IsGreetingDone)
            };

            var playerApproachState = new PlayerApproachState<NpcContext>(
                fsm,
                approachDecision,
                new IState<NpcContext>[] { approachDecision, surpriseState, turnToPlayer, greetingState, idleState },
                subTransitions
            );

            fsm.Initialize(patrolState);

            fsm.AddTransition(new Transition<NpcContext>(
                patrolState,
                playerApproachState,
                ctx => ctx.IsPlayerApproaching
            ));

            fsm.AddTransition(new Transition<NpcContext>(
                playerApproachState,
                patrolState,
                ctx => !ctx.IsPlayerApproaching
            ));
        }

        void Update()
        {
            UpdatePositions();
            UpdateApproachInfo();
            fsm.Update();
        }

        private void UpdatePositions()
        {
            npcContext.NpcPosition = transform.position;
            npcContext.PlayerPosition = playerTransform.position;
        }

        private void UpdateApproachInfo()
        {
            float distance = Vector3.Distance(npcContext.NpcPosition, npcContext.PlayerPosition);
            npcContext.IsPlayerApproaching = (distance < approachDistance);

            Vector3 toPlayer = (playerTransform.position - transform.position).normalized;
            float dot = Vector3.Dot(transform.forward, toPlayer);
            npcContext.IsApproachFromBehind = (dot < behindDotThreshold);
        }
    }
}
