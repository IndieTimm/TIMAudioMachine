using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class MixerModel
{
    public bool[] Invert = new bool[5];
}

public class Mixer : AudioMachineDevice<MixerModel>, IDisplayDataSource<float[]>
{
    private AnalogAudioInterface output = null;
    private AnalogAudioInterface[] inputs = null;
    private List<Button> invertButtons = null;

    public float[] GetData()
    {
        return inputs.Select(x => x.Value).ToArray();
    }

    public override void Process()
    {
        var value = 0F;
        var connected = 0;

        for (int i = 0; i < inputs.Length; i++)
        {
            var input = inputs[i];
            var inverted = state.Invert?.ElementAtOrDefault(i);

            if (input.Connection != null)
            {
                connected++;
            }

            if(inverted.HasValue && inverted.Value)
            {
                value -= input.Value;
            }
            else
            {
                value += input.Value;
            }
        }

        if (connected > 0)
        {
            value /= connected;
        }

        output.Value = value;
    }

    private void SwitchInverted(Button button)
    {
        var index = invertButtons.FindIndex(x => x == button);

        if (index >= 0)
        {
            state.Invert[index] = !state.Invert[index];
        }
    }

    private void Awake()
    {
        output = GetAnalogInterfaces().Find(x => x.Id == "Output");
        inputs = gameObject.GetComponentsInChildren<AnalogAudioInterface>()
            .Where(x => x.Id.Contains("Input"))
            .OrderBy(x => x.Id)
            .ToArray();

        var buttons = gameObject.GetComponentsInChildren<Button>()
            .ToList();

        invertButtons = gameObject.GetComponentsInChildren<Button>()
            .Where(x => x.name.Contains("Invert"))
            .OrderBy(x => x.name)
            .ToList();

        foreach (var button in invertButtons)
        {
            button.RegisterClickCallback(() => SwitchInverted(button));
        }
    }
}
