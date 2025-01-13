namespace Jakkapat.ToppuFSM.Core
{
    public interface IState<TContext>
    {
        bool needsExitTime { get; set; }
        bool CanExit();
        void OnEnter();
        void OnUpdate();
        void OnExit();
    }
}