using Jakkapat.StateMachine.Core;

namespace Jakkapat.StateMachine.Example
{
    public class ApproachInitialSubState
       : BaseState<BaseContext, StateIDs>
    {
        public override StateIDs ID => StateIDs.ApproachInitial;

        private readonly StateMachine<BaseContext, StateIDs> _machine;

        public ApproachInitialSubState(StateMachine<BaseContext, StateIDs> machine)
        {
            _machine = machine;
        }

        public override void UpdateState(BaseContext context)
        {
            if (context.IsPlayerApproachFromBehind())
            {
                context.HasApproached = true;
                _machine.ChangeState(StateIDs.ApproachSurprise);
            }
            else
            {
                context.HasApproached = true;
                context.PlayGreetingAnimation();
                _machine.ChangeState(StateIDs.ApproachGreeting);
            }
        }
    }
}
