
using UnityEngine;
using OscJack;
using System.Collections.Concurrent;

public class OSCReceiverManager : MonoBehaviour
{
    public int port = 39539;

    private OscServer server;

    private readonly ConcurrentQueue<string> messages =
        new ConcurrentQueue<string>();

    private void Start()
    {
        try
        {
            server = new OscServer(port);

            server.MessageDispatcher.AddCallback(
                "",
                OnOSCMessage
            );

            Debug.Log($"[OSC TEST] Listening on UDP {port}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[OSC TEST] ERROR: {e}");
        }
    }

    private void OnOSCMessage(string address, OscDataHandle data)
    {
        string message = $"Address: {address} | Arguments: {data.GetElementCount()}";

        if (data.GetElementCount() >= 2)
        {
            message += $" | Name: {data.GetElementAsString(0)}";
            message += $" | Value: {data.GetElementAsFloat(1)}";
        }

        messages.Enqueue(message);
    }

    private void Update()
    {
        int count = 0;

        while (count < 10 && messages.TryDequeue(out string message))
        {
            Debug.Log("[OSC TEST] " + message);
            count++;
        }
    }

    private void OnDestroy()
    {
        if (server != null)
        {
            server.Dispose();
            server = null;
        }
    }
}
