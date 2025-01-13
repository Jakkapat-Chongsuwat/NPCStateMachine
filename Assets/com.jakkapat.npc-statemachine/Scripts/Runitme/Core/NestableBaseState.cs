using System;

namespace Jakkapat.StateMachine.Core
{
    public abstract class NestableBaseState<TContext, TStateEnum, TSubContext, TSubEnum>
        : BaseState<TContext, TStateEnum>
        where TStateEnum : struct, Enum
        where TSubEnum : struct, Enum
    {
        protected StateMachine<TSubContext, TSubEnum> SubMachine { get; private set; }
        private bool _initialized;

        protected abstract StateMachine<TSubContext, TSubEnum> CreateSubMachine(TContext parentContext);

        public override void EnterState(TContext context, TStateEnum fromState)
        {
            base.EnterState(context, fromState);

            if (!_initialized)
            {
                SubMachine = CreateSubMachine(context);
                _initialized = true;
            }
            InitializeSubMachine(context);
        }

        protected virtual void InitializeSubMachine(TContext parentContext)
        {
            // e.g. SubMachine.ChangeState(...)
        }

        public override void UpdateState(TContext context)
        {
            base.UpdateState(context);
            SubMachine?.Update();
        }

        public override void ExitState(TContext context, TStateEnum toState)
        {
            SubMachine?.ExitCurrentState();
            base.ExitState(context, toState);
        }
    }
}
