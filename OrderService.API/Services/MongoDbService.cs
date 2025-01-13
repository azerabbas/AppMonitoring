using MongoDB.Driver;

namespace OrderService.API.Services
{
    public class MongoDbService
    {
        readonly IMongoDatabase database;
        public MongoDbService(IConfiguration configuration)
        {
            //Database Connection
            var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
            //Database Name
            database = client.GetDatabase("OrderAPIDB");
        }
        // Generic method to get collection
        public IMongoCollection<T> GetCollection<T>() => database.GetCollection<T>(typeof(T).Name.ToLowerInvariant());
    }
}
