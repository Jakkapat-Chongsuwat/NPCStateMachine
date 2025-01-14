namespace Jakkapat.ToppuFSM.Core
{
    public abstract class BaseState<TContext> : IState<TContext>
    {
        protected StateMachine<TContext> stateMachine;
        protected TContext Context => stateMachine.Context;

        public bool needsExitTime { get; set; } = false;

        public virtual bool CanExit()
        {
            return !needsExitTime;
        }

        public BaseState(StateMachine<TContext> parentStateMachine)
        {
            this.stateMachine = parentStateMachine;
        }

        public void SetParentStateMachine(StateMachine<TContext> newParent)
        {
            this.stateMachine = newParent;
        }

        public virtual void OnEnter() { }
        public virtual void OnUpdate() { }
        public virtual void OnExit() { }
    }
}
