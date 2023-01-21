using UnityEngine;

public class JackInteractionStrategyState
{
    public Jack BeginJack;
    public Wire Wire;

    public JackInteractionStrategyState(Jack beginJack, Wire wire)
    {
        BeginJack = beginJack;
        Wire = wire;
    }
}

public class JackInteractionStrategy : InteractionStrategy<JackInteractionStrategyState>
{
    private readonly Jack jack = null;

    public JackInteractionStrategy(Jack jack)
    {
        this.jack = jack;
    }

    protected override JackInteractionStrategyState BeginExecution()
    {
        InterfaceConnectionsManager.Instance.RemoveConnection(jack);

        var wire = Wire.CreateWire(jack.transform, Camera.main.transform);

        return new JackInteractionStrategyState(jack, wire);
    }

    protected override bool CanBeInvoked()
    {
        return Input.GetMouseButtonDown(0);
    }

    protected override void Break(JackInteractionStrategyState state)
    {
        Object.Destroy(state.Wire.gameObject);
    }

    protected override JackInteractionStrategyState Execute(JackInteractionStrategyState state)
    {
        if (state.BeginJack == jack)
        {
            return state;
        }

        if (state.BeginJack != null)
        {
            if (InterfaceConnectionsManager.Instance.TryConnect(state.BeginJack, jack))
            {
                Break(state);
            }
            else
            {
                return state;
            }
        }

        return null;
    }
}
