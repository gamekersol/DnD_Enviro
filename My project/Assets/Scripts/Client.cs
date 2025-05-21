using NativeWebSocket;
using System;
using UnityEngine;

public class Client
{
    private WebSocket websocket;

    public async System.Threading.Tasks.Task Connect(string ip, int port)
    {
        websocket = new WebSocket($"ws://{ip}:{port}");

        websocket.OnOpen += () =>
        {
            Debug.Log("Connected to server");
            Send(new TextMessage("Слава Україні"));
        };
        websocket.OnError += (e) => Debug.LogError("Error: " + e);
        websocket.OnClose += (e) => Debug.Log("Disconnected from server");

        websocket.OnMessage += (bytes) =>
        {
            string message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("Received: " + message);
        };

        await websocket.Connect();
    }

    public void DispatchMessageQueue()
    {
        websocket?.DispatchMessageQueue();
    }

    public async void Disconnect()
    {
        if (websocket != null)
            await websocket.Close();
    }

    public async void Send(MessageBase message)
    {
        if (websocket != null && websocket.State == WebSocketState.Open)
        {
            string json = JsonUtility.ToJson(message);
            var bytes = System.Text.Encoding.UTF8.GetBytes(json);
            await websocket.Send(bytes);
        }
    }
}
