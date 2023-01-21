using System.Linq;

[System.Serializable]
public class OutputAudioInterfaceModel
{
    public float Volume = 0.25F;
    public bool IsMonoMode;
}

public class OutputAudioInterface : AudioMachineDevice<OutputAudioInterfaceModel>, IDisplayDataSource<float[]>
{
    private RotateHandle volumeHandle = null;
    private float[] channels = new float[2];

    public override void Process()
    {
        var interfaces = GetAnalogInterfaces();

        var leftInterface = interfaces.Find(x => x.Id == "L");
        var rightInterface = interfaces.Find(x => x.Id == "R");

        channels[0] = leftInterface.Value * state.Volume;
        channels[1] = (state.IsMonoMode ? leftInterface.Value : rightInterface.Value) * state.Volume;
    }

    public override float GetChannelValue(int channel)
    {
        return channels[channel] / 5;
    }

    public float[] GetData()
    {
        return channels;
    }

    private void SwitchMode()
    {
        state.IsMonoMode = !state.IsMonoMode;
    }

    protected override void Refresh()
    {
        volumeHandle.Value = state.Volume;
    }

    private void Awake()
    {
        IsOutputDevice = true;

        var modeButton = GetComponentsInChildren<Button>().FirstOrDefault(x => x.name == "Mono");
        modeButton?.RegisterClickCallback(SwitchMode);

        volumeHandle = GetComponentInChildren<RotateHandle>();
        volumeHandle.OnValueChanged += (value) => state.Volume = value;
    }
}
