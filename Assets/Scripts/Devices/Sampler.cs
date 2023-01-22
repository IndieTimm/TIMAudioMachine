using System;
using UnityEngine;

[System.Serializable]
public class SamplerModel
{
    
}

public class Sampler : AudioMachineDevice<SamplerModel>
{
    [SerializeField] private AudioClip clip = null;

    private AnalogAudioInterface outputL = null;
    private AnalogAudioInterface outputR = null;
    private float sampleLength;
    private int sampleChannels;
    private float[] data;

    public override void Process()
    {
        var sample = Repeat(AudioMachineTime.Time, sampleLength) / sampleLength;
        var samplesNumber = data.Length / sampleChannels;
        var sampleIndex = ((int)Math.Round(samplesNumber * sample)) * sampleChannels;

        outputL.Value = 5 * data[sampleIndex];
        outputR.Value = 5 * (sampleChannels == 1 ? data[sampleIndex] : data[sampleIndex + 1]);
    }

    private void Awake()
    {
        outputL = GetAnalogInterfaces().Find(x => x.Id == "Output L");
        outputR = GetAnalogInterfaces().Find(x => x.Id == "Output R");

        data = new float[clip.samples * clip.channels];
        sampleLength = clip.length;
        sampleChannels = clip.channels;
        clip.GetData(data, 0);
    }

    private double Repeat(double time, double length)
    {
        return time - Math.Floor(time / length) * length;
    }
}
