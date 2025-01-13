using UnityEngine.AI;

public interface IAgentMovement
{
    NavMeshAgent Agent { get; }
    void StopMovement();
    void SetDestination(UnityEngine.Vector3 position);
    float CurrentSpeed { get; }
    bool IsAtDestination(float tolerance = 0.1f);
}
