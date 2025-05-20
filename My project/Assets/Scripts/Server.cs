using Fleck;
using System.Collections.Generic;
using UnityEngine;

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

            socket.OnMessage = message =>
            {
                Debug.Log("Received from client: " + message);
                // echo or handle message here
            };
        });

        Debug.Log("Server started on port " + port);
    }

    public void Stop()
    {
        foreach (var client in clients)
            client.Close();

        server?.Dispose();
        clients.Clear();
    }
}

