using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace PubSubApp.WebSocketModule
{
    public interface IWebSocketHandler
    {
        Task OnConnected(WebSocket webSocket);
        Task OnDisconnected(WebSocket webSocket);
        Task SendMessageAsync(WebSocket socket, string message);
        Task SendMessageAsync(string socketId, string message);
        Task SendMessageToAllAsync(string message);
        Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
    }
}
