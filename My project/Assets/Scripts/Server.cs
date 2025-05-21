using Fleck;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Server
{
    private WebSocketServer server;
    private List<IWebSocketConnection> clients = new();

    public void Start(int port)
    {
        FleckLog.Level = LogLevel.Warn;
        server = new WebSocketServer($"ws://0.0.0.0:{port}");

        server.Start(socket =>
        {
            socket.OnOpen = () =>
            {
                clients.Add(socket); 
                Debug.Log("Client connected");
            };

            socket.OnClose = () =>
            {
                clients.Remove(socket);
                Debug.Log("Client disconnected");
            };

            socket.OnMessage = message => OnMessage(message);
        });

        Debug.Log("Server started on port " + port);
    }

    private void OnMessage(string msg)
    {
        MessageBase type = JsonUtility.FromJson<MessageBase>(msg);

        switch (type.Type)
        {
            case MessageType.TextMessage:
                TextMessage message = JsonUtility.FromJson<TextMessage>(msg);
                Debug.Log(message.Text);
                break;
            default:
                Debug.Log("Undefined package\n" + msg);
                break;
        }
    }

    public void Stop()
    {
        foreach (var client in clients)
            client.Close();

        server?.Dispose();
        clients.Clear();
    }
}

