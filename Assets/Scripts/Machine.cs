using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Machine : MonoBehaviour
{
    public static int SampleRate = 44100;
    public static bool IsPlaying = true;

    private void OnAudioFilterRead(float[] data, int channels)
    {
        var dataLength = data.Length / channels;
        var timeLength = dataLength / (double)SampleRate;

        for (int i = 0; i < dataLength; i++)
        {
            var time = (i * timeLength) / dataLength + AudioSettings.dspTime;

            AudioMachineTime.Time = time;
            var channelsData = AudioMachineRuntime.GetData();

            for (int channel = 0; channel < channels; channel++)
            {
                data[i * channels + channel] = channelsData[channel];
            }
        }
    }
}