using UnityEngine;
using Jakkapat.StateMachine.Core;

namespace Jakkapat.StateMachine.Example
{
    public class ApproachSurpriseSubState
        : BaseState<NPCContext, StateIDs>
    {
        public override StateIDs ID => StateIDs.ApproachSurprise;

        private readonly StateMachine<NPCContext, StateIDs> _machine;
        private float _timer;
        private float _surpriseDuration = 1.0f;

        public ApproachSurpriseSubState(StateMachine<NPCContext, StateIDs> machine)
        {
            _machine = machine;
        }

        public override void EnterState(NPCContext context, StateIDs fromState)
        {
            _timer = 0f;
            context.HasApproached = true;
            context.RotateToTargetFraction(0.5f);
            context.PlaySurpriseAnimation();
        }

        public override void UpdateState(NPCContext context)
        {
            _timer += Time.deltaTime;
            if (_timer >= _surpriseDuration)
            {
                context.RotateToFaceTarget();
                context.PlayGreetingAnimation();
                _machine.ChangeState(StateIDs.ApproachGreeting);
            }
        }
    }
}
