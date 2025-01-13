namespace Jakkapat.StateMachine.Example
{
    public interface ITargetable
    {
        UnityEngine.Transform SelfTransform { get; }
        UnityEngine.Transform Target { get; }
        bool IsTargetInRange();
    }
}