using UnityEngine;
using Jakkapat.StateMachine.Core;

namespace Jakkapat.StateMachine.Example
{
    public class ApproachGreetingSubState
        : BaseState<NPCContext, StateIDs>
    {
        public override StateIDs ID => StateIDs.ApproachGreeting;

        private readonly StateMachine<NPCContext, StateIDs> _machine;
        private float _timer;
        private float _greetingDuration = 2.0f;

        public ApproachGreetingSubState(StateMachine<NPCContext, StateIDs> machine)
        {
            _machine = machine;
        }

        public override void EnterState(NPCContext context, StateIDs fromState)
        {
            _timer = 0f;
            context.RotateToFaceTarget();
            context.PlayGreetingAnimation();
        }

        public override void UpdateState(NPCContext context)
        {
            _timer += Time.deltaTime;
            context.RotateToFaceTarget();

            if (_timer >= _greetingDuration)
            {
                _machine.ChangeState(StateIDs.ApproachIdle);
            }
        }
    }
}
