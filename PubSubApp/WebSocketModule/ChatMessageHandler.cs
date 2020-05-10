using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using PubSubApp.PubSub;
using Newtonsoft.Json;
using PubSubApp.Models;

namespace PubSubApp.WebSocketModule
{
    public class ChatMessageHandler : WebSocketHandler
    {
        TopicManager topicManager;
        public ChatMessageHandler(ConnectionManager connection) : base(connection)
        {
            topicManager = new TopicManager();
        }

        public override async Task OnConnected(WebSocket webSocket, string userId)
        {
            await base.OnConnected(webSocket);            
            var connectionId = WebSocketConnectionManager.GetId(webSocket);
            topicManager.AddTopicUserSocketMap(userId, userId, connectionId);
            Message message = new Message() { Topic = "All", Msg = $"{userId} is now connected" };
            await SendMessageToAllAsync(JsonConvert.SerializeObject(message));
        }

        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var connectionId = WebSocketConnectionManager.GetId(socket);
            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            Message o = JsonConvert.DeserializeObject<Message>(message);
            //await SendMessageToAllAsync(message);
            await SendMessageAsync(connectionId, JsonConvert.SerializeObject(o));
        }
    }
}
