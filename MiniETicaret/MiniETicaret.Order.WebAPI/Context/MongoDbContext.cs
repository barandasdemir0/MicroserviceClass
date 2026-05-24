using Microsoft.Extensions.Options;
using MiniETicaret.Order.WebAPI.Options;
using MongoDB.Driver;

namespace MiniETicaret.Order.WebAPI.Context;

public sealed class MongoDbContext
{
    private readonly IMongoDatabase _mongoDatabase;

    public MongoDbContext(IOptions<MongoDbSettings> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        _mongoDatabase = client.GetDatabase(options.Value.DatabaseName);
    }

    public IMongoCollection<T> GetCollection<T>(string name)
    {
        return _mongoDatabase.GetCollection<T>(name);
    }
}
