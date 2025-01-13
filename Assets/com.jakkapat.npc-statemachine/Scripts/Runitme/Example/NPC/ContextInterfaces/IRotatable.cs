/// <summary>
/// Provides rotation logic, e.g. TurnSpeed, 
/// or methods to rotate to face the target.
/// </summary>
public interface IRotatable
{
    float TurnSpeed { get; }
    void RotateToFaceTarget();
    void RotateToTargetFraction(float fraction);
}