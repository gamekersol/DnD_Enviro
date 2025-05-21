using System;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance { get; private set; }

    public Client client;
    public Server server;

    public bool isHost;

    private ushort port = 1485;
    private string ip = "127.0.0.1";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public async void StartAsHost(int port)
    {
        isHost = true;
        server = new Server();
        server.Start(port);

        client = new Client();
        await client.Connect("127.0.0.1", port);
    }

    public async void StartAsClient(string ip, int port)
    {
        isHost = false;
        client = new Client();
        await client.Connect(ip, port);
    }

    private void Update()
    {
        client?.DispatchMessageQueue();
    }

    private void OnApplicationQuit()
    {
        client?.Disconnect();
        server?.Stop();
    }

    // UI

    public void OnClickHost()
    {
        StartAsHost(port);
    }

    public void OnClickJoin()
    {
        StartAsClient(ip, port);
    }

    public void SendText(string text) => client.Send(new TextMessage(text));

    public void SetIp(string ip) => this.ip = ip;

    public void SetPort(string port) => this.port = Convert.ToUInt16(port);
}
