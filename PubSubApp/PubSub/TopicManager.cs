using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace PubSubApp.PubSub
{
    public class TopicManager
    {
        private readonly ConcurrentDictionary<string, string> userSocketMap = new ConcurrentDictionary<string, string>();

        public TopicManager()
        {
           
            //var named = (first: "one", second: "two");
            //var sum = 12.5;
            //var count = 5;
            //var accumulation = (count, sum);
        }
    }
}
