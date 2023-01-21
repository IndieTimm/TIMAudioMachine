using System;
using UnityEngine;

public interface IConnection
{
    public string ConnectionA { get; }
    public string ConnectionB { get; }
}

public interface IAnalogConnection : IConnection
{
    float Value { get; set; }
}

public abstract class AnalogAudioInterface : MonoBehaviour, IManagedObject, IIdentificatedObject
{
    public string Id
    {
        get => id;
        set => id = value;
    }

    public string Path
    {
        get
        {
            if (string.IsNullOrEmpty(path))
            {
                path = IdentificatedObjectHelper.GetPath(gameObject);
            }

            return path;
        }
    }

    public IAnalogConnection Connection
    {
        get => connection;
        set => connection = value;
    }

    public float Value
    {
        get => connection?.Value ?? 0F;
        set
        {
            if (connection != null)
            {
                connection.Value = Mathf.Clamp(value, -5, 5);
            }
        }
    }

    [SerializeField] private string id = string.Empty;
    private string path = string.Empty;
    private IAnalogConnection connection = default;

    public string GetId()
    {
        return id;
    }

    public void Save()
    {
        SaveManager.SaveModel(connection, Path);
    }

    public void Load()
    {
        var model = SaveManager.LoadModel(GetModelType(), Path) as IConnection;

        if (model != null)
        {
            var a = InterfaceConnectionsManager.Instance.GetInterface(model.ConnectionA);
            var b = InterfaceConnectionsManager.Instance.GetInterface(model.ConnectionB);

            InterfaceConnectionsManager.Instance.TryConnect(a, b);
        }
    }

    protected abstract Type GetModelType();
}
