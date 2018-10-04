using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace JWT_Test.Models
{
    public class User
    {
        private IMongoDatabase _db;
        private MongoClient _client;

        public User()
        {
            this.createDb();
        }

        public User(string username,
                    string password)
        {
            this.username = username;
            this.password = password;
            this.createDb();

        }

        private void createDb()
        {
            this._client = new MongoClient("");
            this._db = this._client.GetDatabase("");
        }

        public ObjectId _id { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        public bool auth() {

            var user = this._db.GetCollection<User>("Users")
                               .Find(x => x.username == this.username)
                               .FirstOrDefault();

            if (user == null)
                return false;

            return user.password == this.password;
        }

    }
}
