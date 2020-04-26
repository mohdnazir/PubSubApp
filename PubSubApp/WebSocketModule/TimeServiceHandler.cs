using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace PubSubApp.WebSocketModule
{
    public class TimeServiceHandler //: WebSocketHandler
    {
        public TimeServiceHandler(ConnectionManager connection) //: base(connection)
        {

        }
        public /*override*/ Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            throw new NotImplementedException();
        }
    }
}
