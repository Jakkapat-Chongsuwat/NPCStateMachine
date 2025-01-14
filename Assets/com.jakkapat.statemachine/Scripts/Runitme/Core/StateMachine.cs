using System.Collections.Generic;

namespace Jakkapat.ToppuFSM.Core
{
    public class StateMachine<TContext>
    {
        private IState<TContext> currentState;
        private readonly List<ITransition<TContext>> transitions = new List<ITransition<TContext>>();

        public IState<TContext> CurrentState => currentState;

        public TContext Context { get; private set; }

        public StateMachine(TContext context, IState<TContext> initialState)
        {
            Context = context;
            Initialize(initialState);
        }

        public void Initialize(IState<TContext> startingState)
        {
            currentState = startingState;
            currentState?.OnEnter();
        }

        public void Update()
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                var t = transitions[i];
                if (t.FromState == currentState && t.ShouldTransition(Context))
                {
                    if (currentState.CanExit())
                    {
                        ChangeState(t.ToState);
                    }
                    break;
                }
            }

            currentState?.OnUpdate();
        }

        public void ChangeState(IState<TContext> newState)
        {
            if (newState == null || newState == currentState) return;

            currentState?.OnExit();
            currentState = newState;
            currentState?.OnEnter();
        }

        public void AddTransition(ITransition<TContext> transition)
        {
            transitions.Add(transition);
        }

        public void RemoveTransition(ITransition<TContext> transition)
        {
            transitions.Remove(transition);
        }

        public void AddTwoWayTransition(
            IState<TContext> from,
            IState<TContext> to,
            System.Func<TContext, bool> forwardCondition,
            System.Func<TContext, bool> reverseCondition
        )
        {
            var forward = new Transition<TContext>(
                fromState: from,
                toState: to,
                condition: forwardCondition
            );
            AddTransition(forward);

            var reverse = new Transition<TContext>(
                fromState: to,
                toState: from,
                condition: reverseCondition
            );
            AddTransition(reverse);
        }
    }
}
