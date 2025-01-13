using System;
using Jakkapat.StateMachine.Core;
using UnityEngine;

namespace Jakkapat.StateMachine.Example
{
    /// <summary>
    /// A generic PlayerApproachState that can work with 
    /// any TContext + TStateID, provided TContext implements 
    /// the needed interfaces.
    /// 
    /// We also nest a sub-state machine of the *same* TContext & TStateID.
    /// </summary>
    public class PlayerApproachState<TContext, TStateID>
        : NestableBaseState<TContext, TStateID, TContext, TStateID>
        where TContext : class,
                        IContextMachine<TContext, TStateID>,
                        ITargetable,
                        IApproachable,
                        IRotatable,
                        ICanSurprise,
                        ICanGreet,
                        IAgentMovement
        where TStateID : struct, Enum
    {
        private readonly TStateID _id;
        private readonly TStateID _roamingStateID;

        public PlayerApproachState(TStateID id, TStateID roamingID)
        {
            _id = id;
            _roamingStateID = roamingID;
        }

        public override TStateID ID => _id;

        protected override StateMachine<TContext, TStateID> CreateSubMachine(TContext parentContext)
        {
            var subMachine = new StateMachine<TContext, TStateID>(parentContext);

            // Add sub-states also typed for <TContext,TStateID>
            subMachine.AddState(new ApproachInitialSubState<TContext, TStateID>(subMachine));
            subMachine.AddState(new ApproachSurpriseSubState<TContext, TStateID>(subMachine));
            subMachine.AddState(new ApproachGreetingSubState<TContext, TStateID>(subMachine));
            subMachine.AddState(new ApproachIdleSubState<TContext, TStateID>(subMachine));

            return subMachine;
        }

        protected override void InitializeSubMachine(TContext parentContext)
        {
            // Start sub-state => e.g. "ApproachInitial"
            SubMachine.ChangeState(parentContext.ApproachInitialStateID);
            // or use any TStateID you prefer
        }

        public override void EnterState(TContext context, TStateID fromState)
        {
            context.StopMovement();
            base.EnterState(context, fromState);
        }

        public override void UpdateState(TContext context)
        {
            // If out of range => go back to e.g. "Roaming"
            if (!context.IsTargetInRange())
            {
                context.HasApproached = false;
                context.StateMachine.ChangeState(_roamingStateID);
                return;
            }
            base.UpdateState(context);
        }
    }
}
