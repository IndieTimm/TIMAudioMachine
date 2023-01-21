public interface IInteractionStrategy
{
    bool CanBeInoked();
    bool IsForceBreak();
    void Break(object currentState);
    object Invoke(object currentState);
}
