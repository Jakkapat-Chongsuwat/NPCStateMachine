using Jakkapat.StateMachine.Core;

namespace Jakkapat.StateMachine.Example
{
    public class PlayerApproachState
        : NestableBaseState<BaseContext, StateIDs, BaseContext, StateIDs>
    {
        public override StateIDs ID => StateIDs.PlayerApproach;

        protected override StateMachine<BaseContext, StateIDs> CreateSubMachine(BaseContext parentContext)
        {
            var subMachine = new StateMachine<BaseContext, StateIDs>(parentContext);

            subMachine.AddState(new ApproachInitialSubState(subMachine));
            subMachine.AddState(new ApproachSurpriseSubState(subMachine));
            subMachine.AddState(new ApproachGreetingSubState(subMachine));
            subMachine.AddState(new ApproachIdleSubState(subMachine));

            return subMachine;
        }

        protected override void InitializeSubMachine(NPCContext parentContext)
        {
            SubMachine.ChangeState(StateIDs.ApproachInitial);
        }

        public override void EnterState(NPCContext context, StateIDs fromState)
        {
            context.StopMovement();
            base.EnterState(context, fromState);
        }

        public override void UpdateState(NPCContext context)
        {
            if (!context.IsTargetInRange())
            {
                context.HasApproached = false;
                context.StateMachine.ChangeState(StateIDs.Roaming);
                return;
            }
            base.UpdateState(context);
        }
    }
}
