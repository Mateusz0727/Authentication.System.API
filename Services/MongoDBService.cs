using Authentication.System.API.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Linq;
namespace Authentication.System.API.Services;

public class MongoDBService
{
    private readonly IMongoCollection<User> _usersCollection;
    public MongoDBService(IOptions<MongoDBSetting> options)
    {
        MongoClient mongoClient = new MongoClient(options.Value.ConnectionURI);
        IMongoDatabase database = mongoClient.GetDatabase(options.Value.DatabaseName);
        _usersCollection = database.GetCollection<User>(options.Value.CollectionName);
    }
    public  async Task<User> GetByIdAsync(long id)
    {
        return await _usersCollection.Find(x=>x.Id ==id).FirstOrDefaultAsync();
        
    }
   public  async Task CreateAsync(User user)
   {
        await _usersCollection.InsertOneAsync(user);
        return;
   }
}
