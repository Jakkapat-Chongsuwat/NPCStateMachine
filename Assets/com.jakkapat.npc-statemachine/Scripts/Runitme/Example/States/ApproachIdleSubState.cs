using Jakkapat.StateMachine.Core;

namespace Jakkapat.StateMachine.Example
{
    public class ApproachIdleSubState
        : BaseState<NPCContext, StateIDs>
    {
        public override StateIDs ID => StateIDs.ApproachIdle;

        private readonly StateMachine<NPCContext, StateIDs> _machine;

        public ApproachIdleSubState(StateMachine<NPCContext, StateIDs> machine)
        {
            _machine = machine;
        }

        public override void UpdateState(NPCContext context)
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
