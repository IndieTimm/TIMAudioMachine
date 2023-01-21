using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class OscilatorModel
{
    public int Mode = 0;
    public float Modulation = 1F;
    public float OffsetScale = 0.25F;
    public float Amplitude = 0.5F;
    public float Frequency = 16;
}

public class Oscilator : AudioMachineDevice<OscilatorModel>, IDisplayDataSource<float>
{
    [SerializeField] private GameObject[] modeIcons = null;

    private TMPro.TextMeshPro frequencyText = null;
    private string frequencyTextPrefix = string.Empty;
    private AnalogAudioInterface modulationInterface = null;
    private AnalogAudioInterface multiplyInterface = null;
    private AnalogAudioInterface offsetInterface = null;
    private AnalogAudioInterface outputInterface = null;

    public float GetData()
    {
        var freqFactor = (Mathf.Clamp(state.Frequency, 0.01F, 84));
        var multiplyer = 1F / (5.5F * Mathf.Sqrt(freqFactor));

        return GetValue(Time.time * multiplyer);
    }

    public override void Process()
    {
        outputInterface.Value = GetValue(AudioMachineTime.Time);
    }

    private float GetValue(double time)
    {
        var offset = offsetInterface.Value * state.OffsetScale * Mathf.PI;
        var mod =  (1 + modulationInterface.Value / 5) * state.Modulation;
        var phase = offset + time * state.Frequency * mod;

        var value = 0F;
        
        switch(state.Mode)
        {
            case 0: value = OscilationMath.Sin(phase); break;
            case 1: value = OscilationMath.PingPong(phase); break;
            case 2: value = OscilationMath.Quad(phase); break;
        }

        if (multiplyInterface.Connection != null)
        {
            value *= multiplyInterface.Value;
        }

        return Mathf.Clamp(value * state.Amplitude, -5F, 5F);
    }

    private void SwitchMode()
    {
        state.Mode = (state.Mode + 1) % 3;
        Refresh();
    }

    private void ChangeFrequency(float delta)
    {
        state.Frequency += delta;
        Refresh();
    }

    protected override void Refresh()
    {
        for (int i = 0; i < modeIcons.Length; i++)
        {
            modeIcons[i].SetActive(i == state.Mode);
        }

        if (frequencyText != null)
        {
            frequencyText.text = frequencyTextPrefix + state.Frequency.ToString("0.00");
        }
    }

    private void Awake()
    {
        modulationInterface = GetAnalogInterfaces().Find(x => x.Id == "Mod");
        multiplyInterface = GetAnalogInterfaces().Find(x => x.Id == "Mul");
        offsetInterface = GetAnalogInterfaces().Find(x => x.Id == "Offset");
        outputInterface = GetAnalogInterfaces().Find(x => x.Id == "Output");
        frequencyText = GetComponentsInChildren<TMPro.TextMeshPro>().FirstOrDefault(x => x.name == "FrequencyText");

        var modeButton = GetComponentsInChildren<Button>().FirstOrDefault(x => x.name == "Mode");
        modeButton?.RegisterClickCallback(SwitchMode);

        var upButton = GetComponentsInChildren<Button>().FirstOrDefault(x => x.name == "Up");
        upButton?.RegisterClickCallback(() => ChangeFrequency(1));

        var downButton = GetComponentsInChildren<Button>().FirstOrDefault(x => x.name == "Down");
        downButton?.RegisterClickCallback(() => ChangeFrequency(-1));

        if (frequencyText != null)
        {
            frequencyTextPrefix = frequencyText.text;
            Refresh();
        }
    }
}
