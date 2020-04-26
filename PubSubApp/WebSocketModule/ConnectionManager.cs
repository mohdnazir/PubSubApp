using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace PubSubApp.WebSocketModule
{
    /// <summary>
    /// https://radu-matei.com/blog/aspnet-core-websockets-middleware/
    /// </summary>
    public class ConnectionManager
    {
        private readonly ConcurrentDictionary<string, WebSocket> socketList = new ConcurrentDictionary<string, WebSocket>();

        public WebSocket GetWebSocketByID(string id)
        {
            return socketList.FirstOrDefault(p => p.Key == id).Value;
        }

        public ConcurrentDictionary<string, WebSocket> GetAll()
        {
            return socketList;
        }

        public string GetId(WebSocket socket)
        {
            return socketList.FirstOrDefault(s => s.Value == socket).Key;
        }

        public void AddSocket(WebSocket webSocket)
        {
            socketList.TryAdd(CreateConnectionId(), webSocket);
        }

        public async Task RemoveSocket(string connectionId)
        {
            WebSocket webSocket;
            socketList.TryRemove(connectionId, out webSocket);

            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure,
                statusDescription: "Closed by connection manager",
                cancellationToken: CancellationToken.None);
        }

        private static string CreateConnectionId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
