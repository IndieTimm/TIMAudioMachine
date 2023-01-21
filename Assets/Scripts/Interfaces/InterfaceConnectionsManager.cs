using System.Collections.Generic;
using System.Linq;

public class InterfaceConnectionsManager : StaticInstanceBehaviour<InterfaceConnectionsManager>
{
    private List<AnalogAudioInterface> analogInterfaces = new List<AnalogAudioInterface>();

    public override void Initialization()
    {
        analogInterfaces = FindObjectsOfType<AnalogAudioInterface>().ToList();
    }

    public AnalogAudioInterface GetInterface(string path)
    {
        return analogInterfaces.Find(x => x.Path == path);
    }

    public bool Is(string path, AnalogAudioInterface deviceInterface)
    {
        return deviceInterface.Path == path;
    }

    public bool TryConnect(AnalogAudioInterface interfaceA, AnalogAudioInterface interfaceB)
    {
        if (interfaceA == interfaceB || (interfaceA.Connection != null || interfaceB.Connection != null))
        {
            return false;
        }

        var connection = new JackConnection()
        {
            ConnectionA = interfaceA.Path,
            ConnectionB = interfaceB.Path
        };

        interfaceA.Connection = connection;
        interfaceB.Connection = connection;

        WireDrawingManager.Instance.Create(connection);
        AudioMachineRuntime.Refresh();

        return true;
    }

    public void RemoveConnection(AnalogAudioInterface audioInterface)
    {
        var interfaces = analogInterfaces.FindAll(x => 
            x.Connection != null &&
            (x.Connection.ConnectionA == audioInterface.Path || 
            x.Connection.ConnectionB == audioInterface.Path));

        foreach (var @interface in interfaces)
        {
            WireDrawingManager.Instance.Remove(@interface.Connection);
            @interface.Connection = null;
        }

        AudioMachineRuntime.Refresh();
    }
}
