/// <summary>
/// Provides a property or method for greeting logic.
/// </summary>
public interface ICanGreet
{
    void PlayGreetingAnimation();
    bool HasGreeted { get; set; }
}