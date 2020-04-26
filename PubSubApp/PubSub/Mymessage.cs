namespace PubSubApp.PubSub
{
    public class Mymessage
    {
        string msg;
        public Mymessage(string message)
        {
            msg = message;
        }
        public override string ToString()
        {
            return msg;
        }
    }
}