namespace MyGame.StateMachineFramework
{
    public abstract class BaseState<TContext> : IState<TContext>
    {
        protected StateMachine<TContext> stateMachine;
        protected TContext Context => stateMachine.Context;

        // The constructor sets the parent SM if known at creation time
        public BaseState(StateMachine<TContext> parentStateMachine)
        {
            this.stateMachine = parentStateMachine;
        }

        // Add this method so we can set/override the state machine reference if needed
        public void SetParentStateMachine(StateMachine<TContext> newParent)
        {
            this.stateMachine = newParent;
        }

        public virtual void OnEnter() { }
        public virtual void OnUpdate() { }
        public virtual void OnExit() { }
    }
}
