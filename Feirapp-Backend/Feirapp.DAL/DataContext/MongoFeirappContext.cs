using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Feirapp.DAL.DataContext;

public interface IMongoFeirappContext
{
    IMongoCollection<T> GetCollection<T>(string name);

    void DropCollection(string name);
}

public class MongoFeirappContext : IMongoFeirappContext
{
    private readonly MongoClient _client;
    private readonly IMongoDatabase _database;

    public MongoFeirappContext(IOptions<MongoSettings> configuration)
    {
        _client = new MongoClient(configuration.Value.ConnectionString);
        _database = _client.GetDatabase(configuration.Value.DatabaseName);
    }

    public void DropCollection(string name)
    {
        _database.DropCollection(name);
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }
}