
public interface IApproachable
{
    bool HasApproached { get; set; }
    float ApproachRange { get; set; }
    bool IsPlayerApproachFromBehind();
}
