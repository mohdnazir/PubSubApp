using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace PubSubApp.PubSub
{
    public class TopicManager
    {
        private readonly ConcurrentDictionary<string, List<(string userid, string connectionId)>>
            topickUserSocketMap = new ConcurrentDictionary<string, List<(string userid, string connectionId)>>();

        public TopicManager()
        {           
            //var named = (first: "one", second: "two");
            //var sum = 12.5;
            //var count = 5;
            //var accumulation = (count, sum);
        }

        public void AddTopicUserSocketMap(string topic, string userid, string connectionId)
        {
            var ps = new List<(string userid, string connectionId)>();
            ps.Add((userid, connectionId));
            topickUserSocketMap.AddOrUpdate(topic, ps, (key, existingVal) =>
            {
                existingVal.Add(ps.FirstOrDefault());
                return existingVal;
            });
        }

        public List<(string userid, string connectionId)> GetUserSocketMap(string topic)
        {
            return topickUserSocketMap.GetValueOrDefault(topic);
        }

    }
}
