using UnityEngine;

[RequireComponent(typeof(Mixer))]
public class MixerInvertedStateDataSource : MonoBehaviour, IDisplayDataSource<bool[]>
{
    private Mixer mixer = null;

    public bool[] GetData()
    {
        return mixer.State.Invert;
    }

    private void Awake()
    {
        mixer = GetComponent<Mixer>();    
    }
}
