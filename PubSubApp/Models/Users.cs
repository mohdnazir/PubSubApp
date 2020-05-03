using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PubSubApp.Models
{
    //mongodb+srv://sa:<password>@pubsubapp-kf887.mongodb.net/test?retryWrites=true&w=majority
    //public class Users
    //{
    //    [BsonId]
    //    [BsonRepresentation(BsonType.ObjectId)]
    //    public string Id { get; set; }
    //    public string DisplayName { get; set; }
    //    public string UserID { get; set; }
    //    public string SocketId { get; set; }

    //}

    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string UserID { get; set; }
        public string Status { get; set; }
        public string SocketId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
