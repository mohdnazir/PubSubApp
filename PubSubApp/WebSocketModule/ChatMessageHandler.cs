using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace PubSubApp.WebSocketModule
{
    public class ChatMessageHandler : WebSocketHandler
    {
        public ChatMessageHandler(ConnectionManager connection) : base(connection)
        {

        }

        public override async Task OnConnected(WebSocket webSocket, string userId)
        {
            await base.OnConnected(webSocket);

            var connectionId = WebSocketConnectionManager.GetId(webSocket);
            await SendMessageToAllAsync($"{connectionId} is now connected");
        }

        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var connectionId = WebSocketConnectionManager.GetId(socket);
            var message = $"{connectionId} said: {Encoding.UTF8.GetString(buffer, 0, result.Count)}";

            await SendMessageToAllAsync(message);
        }
    }
}
