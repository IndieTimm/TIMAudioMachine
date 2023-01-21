using UnityEngine;

public class RuntimeManager : MonoBehaviour
{
    private void Start()
    {
        foreach (var managedObject in GameSceneUtility.FindObjects<IManagedObject>())
        {
            managedObject.Load();
        }
    }
}
