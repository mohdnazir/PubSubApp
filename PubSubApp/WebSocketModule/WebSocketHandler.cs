using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PubSubApp.WebSocketModule
{
    public abstract class WebSocketHandler : IWebSocketHandler
    {
        protected ConnectionManager WebSocketConnectionManager { get; set; }

        public WebSocketHandler(ConnectionManager connectionManager)
        {
            WebSocketConnectionManager = connectionManager;
        }

        public virtual async Task OnConnected(WebSocket webSocket)
        {
            WebSocketConnectionManager.AddSocket(webSocket);
        }

        public virtual async Task OnDisconnected(WebSocket webSocket)
        {
            await WebSocketConnectionManager.RemoveSocket(WebSocketConnectionManager.GetId(webSocket))
                .ConfigureAwait(false);
        }

        public async Task SendMessageAsync(WebSocket socket, string message)
        {
            if (socket == null)
                throw new ArgumentNullException(nameof(socket));
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException(nameof(message));

            if (socket.State != WebSocketState.Open)
                return;

            await socket.SendAsync(buffer: new ArraySegment<byte>(array: Encoding.ASCII.GetBytes(message),
                                                                offset: 0,
                                                                count: message.Length),
                                                                messageType: WebSocketMessageType.Text,
                                                                endOfMessage: true,
                                                                cancellationToken: CancellationToken.None)
                .ConfigureAwait(false);

        }

        public async Task SendMessageAsync(string socketId, string message)
        {
            await SendMessageAsync(WebSocketConnectionManager.GetWebSocketByID(socketId), message)
                .ConfigureAwait(false);
        }

        public async Task SendMessageToAllAsync(string message)
        {
            foreach (var item in WebSocketConnectionManager.GetAll())
            {
                if (item.Value.State == WebSocketState.Open)
                    await SendMessageAsync(item.Value, message)
                        .ConfigureAwait(false);

            }
        }

        public abstract Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
    }
}
