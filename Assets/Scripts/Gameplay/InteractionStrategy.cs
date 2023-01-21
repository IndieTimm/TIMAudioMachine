public abstract class InteractionStrategy<TState> : IInteractionStrategy
{
    public bool CanBeInoked()
    {
        return CanBeInvoked();
    }

    public void Break(object currentState)
    {
        if (currentState is TState state)
        {
            Break(state);
        }
    }

    public object Invoke(object currentState)
    {
        if (currentState == null)
        {
            return BeginExecution();
        }

        if (currentState is TState state)
        {
            return Execute(state);
        }

        return null;
    }

    protected virtual void Break(TState state)
    {

    }

    public virtual bool IsForceBreak()
    {
        return false;
    }

    protected abstract bool CanBeInvoked();
    protected abstract TState BeginExecution();
    protected abstract TState Execute(TState state);
}
