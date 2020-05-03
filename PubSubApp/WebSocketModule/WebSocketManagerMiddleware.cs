using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace PubSubApp.WebSocketModule
{
    public class WebSocketManagerMiddleware
    {
        private readonly RequestDelegate _next;
        private IWebSocketHandler WebSocketHandler { get; set; }

        public WebSocketManagerMiddleware(RequestDelegate next, IWebSocketHandler handler)
        {
            _next = next;
            WebSocketHandler = handler;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (!httpContext.WebSockets.IsWebSocketRequest)
                return;
            string userId = httpContext.Request.Query["userid"];
            var socket = await httpContext.WebSockets.AcceptWebSocketAsync();
            await WebSocketHandler.OnConnected(socket, userId);

            await Receive(socket, async (result, buffer) =>
            {
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    await WebSocketHandler.ReceiveAsync(socket, result, buffer).ConfigureAwait(false);
                    return;
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await WebSocketHandler.OnDisconnected(socket).ConfigureAwait(false);
                    return;
                }

            }).ConfigureAwait(false);

        }

        private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> action)
        {
            var buffer = new byte[1024 * 4];
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer),
                    cancellationToken: CancellationToken.None).ConfigureAwait(false);

                action(result, buffer);
            }
        }
    }
}
