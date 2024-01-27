using Authentication.System.API.Models;
using Authentication.System.API.Models.Settings;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

using ParkBee.MongoDb;
using MongoDB.Driver;
using Authentication.System.API.Extensions;

namespace Authentication.System.API.Data
{
    public class BaseContext
    {
        private readonly IMongoClient _mongoClient;
        private readonly IMongoDatabase _database;

        public BaseContext(string connectionString, string databaseName)
        {
            _mongoClient = new MongoClient(connectionString);
            _database = _mongoClient.GetDatabase(databaseName);
          
        }

        // You can add properties for your MongoDB collections here, e.g., as follows:
        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
       /* public IMongoCollection<T> DbSet<T>() where T : class
        {
        }*/
        public async Task Add<T>(T entity, CancellationToken cancellationToken)
        {

            var table = typeof(T).GetCustomAttributes(typeof(TableAttribute), false)
                                .Cast<TableAttribute>()
                                .FirstOrDefault()?.GetName();

           var _collection= _database.GetCollection<T>(table);

            await _collection.InsertOneAsync(entity, cancellationToken);
        }
       /* public object Create<T>(T model)
        {
            try
            {
                Collection.InsertOne(model);
                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }
        }*/
    }
}
    /* public IMongoCollection<User> UserRecord
     {
         get
         {
           IMongoCollection<User>  _usersCollection= _database.GetCollection<User>("user");
             var uniqueFieldIndex = Builders<User>.IndexKeys.Ascending("Email");
             var indexOptions = new CreateIndexOptions { Unique = true };


             _usersCollection.Indexes.CreateOne(uniqueFieldIndex, indexOptions);
             return _usersCollection;
         }
     }*/
