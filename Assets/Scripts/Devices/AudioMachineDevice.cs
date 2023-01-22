using System.Collections.Generic;
using UnityEngine;

public abstract class AudioMachineDevice : MonoBehaviour
{
    public bool IsOutputDevice { get; set; }

    private List<AnalogAudioInterface> analogInterfaces = new List<AnalogAudioInterface>();

    public bool HasInterface(AnalogAudioInterface audioInterface)
    {
        return analogInterfaces.Contains(audioInterface);
    }

    public virtual List<AnalogAudioInterface> GetAnalogInterfaces()
    {
        if (analogInterfaces.Count == 0)
        {
            analogInterfaces.AddRange(GetComponentsInChildren<AnalogAudioInterface>());
        }

        return analogInterfaces;
    }

    public virtual float GetChannelValue(int channel)
    {
        return 0;
    }

    public abstract void Process();
}

public abstract class AudioMachineDevice<TState> : AudioMachineDevice, IManagedObject, IIdentificatedObject
{
    public TState State
    {
        get => state;
    }

    [SerializeField] protected TState state = default;
    [SerializeField] private string id = "Default_Device";

    public object GetModel()
    {
        return state;
    }

    public void Save()
    {
        SaveManager.SaveModel(state, id);
    }

    public void Load()
    {
        var model = SaveManager.LoadModel<TState>(id);

        if (model != null)
        {
            state = model;
            Refresh();
        }
    }

    protected virtual void Refresh() { }

    public string GetId()
    {
        return id;
    }
}
