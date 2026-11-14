
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class UDPDiagnostic : MonoBehaviour
{
    [SerializeField] private int port = 39540;

    private UdpClient udp;
    //gg
    private readonly ConcurrentQueue<string> messages =
        new ConcurrentQueue<string>();

    private int packetCount;

    private void Start()
    {
        try
        {
            udp = new UdpClient(
                new IPEndPoint(IPAddress.Any, port));

            Debug.Log(
                $"[UDP TEST] Listening on port {port}");

            udp.BeginReceive(ReceiveCallback, null);
        }
        catch (Exception e)
        {
            Debug.LogError(
                "[UDP TEST] Could not start: " + e.Message);
        }
    }

    private void ReceiveCallback(IAsyncResult result)
    {
        try
        {
            IPEndPoint sender = new IPEndPoint(
                IPAddress.Any, 0);

            byte[] data = udp.EndReceive(result, ref sender);

            packetCount++;

            string preview = Encoding.ASCII.GetString(
                data, 0, Math.Min(data.Length, 80));

            string hex = BitConverter.ToString(
                data, 0, Math.Min(data.Length, 32));

            messages.Enqueue(
                $"Packet #{packetCount} | " +
                $"From {sender.Address}:{sender.Port}\n" +
                $"Bytes: {data.Length}\n" +
                $"ASCII: {preview}\n" +
                $"HEX: {hex}");

            udp.BeginReceive(ReceiveCallback, null);
        }
        catch (ObjectDisposedException)
        {
            // Listener was stopped.
        }
        catch (Exception e)
        {
            messages.Enqueue("[UDP ERROR] " + e.Message);
        }
    }

    private void Update()
    {
        while (messages.TryDequeue(out string message))
        {
            Debug.Log(message);
        }
    }

    private void OnDestroy()
    {
        udp?.Close();
        udp = null;
    }
}
