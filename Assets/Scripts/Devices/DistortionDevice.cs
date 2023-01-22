using UnityEngine;

[System.Serializable]
public class DistortionModel
{
    public float Cutout;
    public bool OneMode;
}

public class DistortionDevice : AudioMachineDevice<DistortionModel>
{
    private AnalogAudioInterface input = null;
    private AnalogAudioInterface output = null;

    public override void Process()
    {
        var value = input.Value;
        var level = state.Cutout * 5F;

        value = Mathf.Clamp(value, -level, level);

        output.Value = value;
    }

    private void Awake()
    {
        var level = GetComponentInChildren<RotateHandle>();
        input = GetAnalogInterfaces().Find(x => x.Id == "Input");
        output = GetAnalogInterfaces().Find(x => x.Id == "Output");

        level.OnValueChanged += ValueChange;
    }

    private void ValueChange(float value)
    {
        state.Cutout = value;
    }
}
