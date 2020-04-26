using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PubSubApp.PubSub
{
    public class Subscriber
    {
        Subscription<Mymessage> myMessageToken;
        Subscription<int> intToken;
        EventAggregator eventAggregator;

        public Subscriber(EventAggregator eve)
        {
            eventAggregator = eve;
            eve.Subscribe<Mymessage>(this.Test);
            eve.Subscribe<int>(this.IntTest);
        }

        private void IntTest(int obj)
        {
            Console.WriteLine(obj);
            eventAggregator.UnSbscribe(intToken);
        }

        private void Test(Mymessage test)
        {
            Console.WriteLine(test.ToString());
            Debug.WriteLine(test.ToString());
            eventAggregator.UnSbscribe(myMessageToken);
        }
    }
}
