using MongoDB.Driver;
using PubSubApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PubSubApp.Services
{
    public class UserService
    {
        readonly IMongoCollection<User> userList;
        public UserService(IPubSubAppDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            userList = database.GetCollection<User>(settings.UserCollectionName);
        }
        public List<User> Get() => userList.Find(u => true).ToList();
        public User Get(string id) => userList.Find(u => u.UserID == id).FirstOrDefault();
        public User Create(User user)
        {
            var existingUser = Get(user.UserID);
            if (existingUser != null)
            {
                user.Id = existingUser.Id;
                user.ModifiedAt = DateTime.Now;
                user.CreatedAt = existingUser.CreatedAt;
                userList.ReplaceOne(u => u.Id == existingUser.Id, user);
            }
            else
            {
                user.CreatedAt = DateTime.Now;
                user.ModifiedAt = DateTime.Now;
                userList.InsertOne(user);
            }
            
            return user;
        }
        public void Update(string id, User userIn) => userList.ReplaceOne(u => u.UserID == id, userIn);
        public void Remove(User user) => userList.DeleteOne(du => du.UserID == user.UserID);
        public void Remove(string id) => userList.DeleteOne(du => du.UserID == id);
    }
}
