namespace Jakkapat.ToppuFSM.Core
{
    public class Transition<TContext> : ITransition<TContext>
    {
        public IState<TContext> FromState { get; }
        public IState<TContext> ToState { get; }
        private readonly System.Func<TContext, bool> condition;

        public Transition(
            IState<TContext> fromState,
            IState<TContext> toState,
            System.Func<TContext, bool> condition)
        {
            FromState = fromState;
            ToState = toState;
            this.condition = condition;
        }

        public bool ShouldTransition(TContext context)
        {
            return condition != null && condition.Invoke(context);
        }
    }
}
