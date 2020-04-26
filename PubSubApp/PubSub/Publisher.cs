using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PubSubApp.PubSub
{
    public class Publisher
    {
        EventAggregator EventAggregator;
        public Publisher(EventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;
        }

        public void PublishMessage(Mymessage mymessage)
        {
            EventAggregator.Publish(mymessage);
            //EventAggregator.Publish(10);k
        }
    }
}
