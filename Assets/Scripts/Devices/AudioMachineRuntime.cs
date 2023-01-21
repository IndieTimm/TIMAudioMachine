using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AudioMachineRuntime
{
    private static List<List<AudioMachineDevice>> deviceLayers = new List<List<AudioMachineDevice>>();
    private static float[] channels = new float[2];

    public static float[] GetData()
    {
        for (int i = 0; i < deviceLayers.Count; i++)
        {
            foreach(var device in deviceLayers[i])
            {
                device.Process();
            }
        }

        if (deviceLayers.Count > 0)
        {
            channels[0] = deviceLayers.Last()[0].GetChannelValue(0);
            channels[1] = deviceLayers.Last()[0].GetChannelValue(1);
        }

        return channels;
    }

    public static void Refresh()
    {
        deviceLayers.Clear();

        var devices = GameSceneUtility.FindObjects<AudioMachineDevice>()
            .ToList();

        var rootDevices = devices
            .FindAll(x => x.IsOutputDevice)
            .Take(1)
            .ToList();

        deviceLayers.Add(rootDevices);
        devices.RemoveAll(x => x.IsOutputDevice);

        var interation = 0;

        for (; interation < 1000; interation++)
        {
            var previousLayer = deviceLayers.LastOrDefault();
            var currentLayer = new List<AudioMachineDevice>();

            if (!previousLayer.Any())
            {
                break;
            }

            foreach (var device in previousLayer)
            {
                foreach (var deviceInterface in device.GetAnalogInterfaces())
                {
                    if (deviceInterface.Connection == null)
                    {
                        continue;
                    }

                    var connectedToInterface = InterfaceConnectionsManager.Instance.Is(deviceInterface.Connection.ConnectionA, deviceInterface)
                        ? InterfaceConnectionsManager.Instance.GetInterface(deviceInterface.Connection.ConnectionB)
                        : InterfaceConnectionsManager.Instance.GetInterface(deviceInterface.Connection.ConnectionA);

                    var connectedToDeviceIndex = devices.FindIndex(x => x.HasInterface(connectedToInterface));

                    if (connectedToDeviceIndex >= 0)
                    {
                        currentLayer.Add(devices[connectedToDeviceIndex]);
                        devices.RemoveAt(connectedToDeviceIndex);
                    }
                }
            }

            if (currentLayer.Any())
            {
                deviceLayers.Add(currentLayer);
            }
            else
            {
                break;
            }
        }

        deviceLayers.Reverse();

        Debug.Log($"Audio machine runtime refreshed. Iterations number:{interation}");
        for (int i = 0; i < deviceLayers.Count; i++)
        {
            List<AudioMachineDevice> layer = deviceLayers[i];
            Debug.Log($"Layer{i} have {layer.Count} devices");
        }
    }
}
