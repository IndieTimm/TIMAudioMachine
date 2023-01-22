using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class MidiNoteInfo
{
    public float StartTime;
    public float Length;
    public int Note;
}

[System.Serializable]
public class MidiPlayer
{
    [SerializeField] private bool IsRepeate = true;
    [SerializeField] private MidiNoteInfo[] notes = new MidiNoteInfo[0];

    public MidiNoteInfo[] GetNotes(float time)
    {
        if (IsRepeate)
        {
            var begin = notes.Min(x => x.StartTime);
            var end = notes.Max(x => x.StartTime + x.Length);

            time = begin + Mathf.Repeat(time, end - begin);
        }

        return notes.Where(x => IsCross(time, x))
            .ToArray();
    }

    private bool IsCross(float time, MidiNoteInfo info)
    {
        var t = time - info.StartTime;

        return t >= 0 && t <= info.Length;
    }
}

public static class PianoHelper
{
    private const float frequency = 440;

    public static float GetFrequency(int note)
    {
        var value = frequency * Mathf.Pow(2, note / 12F);

        return Mathf.Round(value * 100) / 100;
    }
}

[System.Serializable]
public class AnalogPianoModel
{
    public int Octave = 0;
}

public class AnalogPiano : AudioMachineDevice<AnalogPianoModel>
{
    [SerializeField] private bool isPlaying;
    [SerializeField] private MidiPlayer player = null;
    [SerializeField] private float angle = 10;
    [SerializeField] private Vector3 axis = Vector3.right;
    [SerializeField] private int noteIndexOffset = 0;
    [SerializeField] private List<GameObject> keys = new List<GameObject>();

    private AnalogAudioInterface outputInterface = null;
    private List<int> holdedKeys = new List<int>();

    public override void Process()
    {
        var value = 0F;

        foreach (var index in holdedKeys)
        {
            value += GetValue(AudioMachineTime.Time, index + noteIndexOffset + state.Octave * 13);
        }

        outputInterface.Value = value;
    }

    private void Awake()
    {
        var playButton = GetComponentsInChildren<Button>().FirstOrDefault(x => x.name == "Play");
        playButton?.RegisterClickCallback(PlayStop);

        var upButton = GetComponentsInChildren<Button>().FirstOrDefault(x => x.name == "Up");
        upButton?.RegisterClickCallback(() => ChangeOctave(1));

        var downButton = GetComponentsInChildren<Button>().FirstOrDefault(x => x.name == "Down");
        downButton?.RegisterClickCallback(() => ChangeOctave(-1));

        for (int i = 0; i < keys.Count; i++)
        {
            var key = keys[i];
            var button = key.GetComponent<Button>();

            if (button != null)
            {
                button.RegisterClickCallback(() => PressKeyButton(key));
            }
        }

        outputInterface = GetAnalogInterfaces().Find(x => x.Id == "Output");
    }

    private void PlayStop()
    {
        isPlaying = !isPlaying;

        if (!isPlaying)
        {
            holdedKeys.Clear();
        }
    }

    private void ChangeOctave(int delta)
    {
        state.Octave += delta;
        Refresh();
    }

    protected override void Refresh()
    {
        var frequencyText = GetComponentsInChildren<TMPro.TextMeshPro>().FirstOrDefault(x => x.name == "Octave");

        if (frequencyText != null)
        {
            if (state.Octave > 0)
            {
                frequencyText.text = "+";
            }
            else
            {
                frequencyText.text = string.Empty;
            }

            frequencyText.text += state.Octave;
        }
    }

    private void Update()
    {
        for (int i = 0; i < keys.Count; i++)
        {
            var key = keys[i];
            var angle = holdedKeys.Contains(i)
                ? this.angle
                : 0;

            key.transform.localRotation = Quaternion.Lerp(key.transform.localRotation, Quaternion.AngleAxis(angle, axis), 10F * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        if (isPlaying)
        {
            holdedKeys = player.GetNotes(Time.time).Select(x => x.Note).ToList();
        }
    }

    private void PressKeyButton(GameObject key)
    {
        var index = keys.FindIndex(x => x == key);

        if (index == -1)
        {
            return;
        }

        if (holdedKeys.Contains(index))
        {
            holdedKeys.Remove(index);
        }
        else
        {
            holdedKeys.Add(index);
        }

        
    }

    private float GetValue(double time, int noteIndex)
    {
        var phase = time * PianoHelper.GetFrequency(noteIndex);

        var value = OscilationMath.Sin(phase) * 5;

        return Mathf.Clamp(value, -5F, 5F);
    }
}
