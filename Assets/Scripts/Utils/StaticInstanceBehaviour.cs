using UnityEngine;

public abstract class StaticInstanceBehaviourBase : MonoBehaviour
{
    public virtual void Initialization()
    {
    }
}

/// <summary>
/// Gives access to instance of T inherit from MonoBehaviour.
/// If instance of T object wasn't be founded, throws exception.
/// </summary>
public abstract class StaticBehaviour<T> : StaticInstanceBehaviourBase where T : StaticInstanceBehaviourBase
{
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GetInstanceOrThrow();
                instance.Initialization();
            }

            return instance;
        }
    }

    private static T instance = null;

    private static T GetInstanceOrThrow()
    {
        T currentInstace = FindObjectOfType<T>();

        if (currentInstace == null)
        {
            throw new System.Exception($"Instance of {typeof(T)} object wasn't found.");
        }

        return currentInstace;
    }
}

/// <summary>
/// Gives access to instance of T inherit from MonoBehaviour.
/// If instance of T object wasn't be founded, creates one.
/// </summary>
public abstract class StaticInstanceBehaviour<T> : StaticInstanceBehaviourBase where T : StaticInstanceBehaviourBase
{
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GetOrCreateInstance();
                instance.Initialization();
            }

            return instance;
        }
    }

    private static T instance = null;

    private static T GetOrCreateInstance()
    {
        T currentInstace = FindObjectOfType<T>();

        if (currentInstace == null)
        {
            GameObject instanceContainer = new GameObject($"{typeof(T)} Instance");

            currentInstace = instanceContainer.AddComponent<T>();
        }

        return currentInstace;
    }
}