using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PubSubApp.Models
{
    public class Message
    {
        public string Topic { get; set; }
        public string Msg { get; set; }
    }
}
