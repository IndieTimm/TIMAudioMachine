using System.Collections.Generic;
using System.Linq;

public class WireDrawingManager : StaticInstanceBehaviour<WireDrawingManager>
{
    private List<Wire> wires = new List<Wire>();

    public void Remove(IConnection connection)
    {
        var a = InterfaceConnectionsManager.Instance.GetInterface(connection.ConnectionA);
        var b = InterfaceConnectionsManager.Instance.GetInterface(connection.ConnectionB);
        var removedWires = wires.FindAll(x => 
            (x.From == a.transform && x.To == b.transform) || 
            (x.From == b.transform && x.To == a.transform));

        foreach (var wire in removedWires)
        {
            wires.Remove(wire);
            Destroy(wire.gameObject);
        }
    }

    public void Create(IConnection connection)
    {
        var a = InterfaceConnectionsManager.Instance.GetInterface(connection.ConnectionA);
        var b = InterfaceConnectionsManager.Instance.GetInterface(connection.ConnectionB);
        var wire = Wire.CreateWire(a.transform, b.transform);

        if (wire != null)
        {
            wires.Add(wire);
        }
    }
}
