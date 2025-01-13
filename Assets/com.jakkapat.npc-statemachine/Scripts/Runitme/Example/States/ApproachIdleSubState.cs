using Jakkapat.StateMachine.Core;

namespace Jakkapat.StateMachine.Example
{
    public class ApproachIdleSubState
        : BaseState<NpcContext, StateIDs>
    {
        public override StateIDs ID => StateIDs.ApproachIdle;

        private readonly StateMachine<NpcContext, StateIDs> _machine;

        public ApproachIdleSubState(StateMachine<NpcContext, StateIDs> machine)
        {
            _machine = machine;
        }

        public override void UpdateState(NpcContext context)
        {
            if (context.IsTargetInRange())
            {
                context.RotateToFaceTarget();
            }
            else
            {
                // Possibly do nothing or revert to Idle
            }
        }
    }
}
