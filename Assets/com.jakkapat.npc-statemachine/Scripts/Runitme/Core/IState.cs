namespace MyGame.StateMachineFramework
{
    public interface IState<TContext>
    {
        void OnEnter();
        void OnUpdate();
        void OnExit();
    }
}
