using System;
using UnityEngine;

[System.Serializable]
public class JackConnection : IAnalogConnection
{
    public string ConnectionA { get; set; }
    public string ConnectionB { get; set; }
    public float Value { get; set; }
}

public class Jack : AnalogAudioInterface, IInteractableObject
{
    public IInteractionStrategy GetStrategy()
    {
        return new JackInteractionStrategy(this);
    }

    protected override Type GetModelType()
    {
        return typeof(JackConnection);
    }
}
