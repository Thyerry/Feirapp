using Feirapp.Domain.Models;
using Microsoft.VisualBasic;
using MongoDB.Driver;
using Moq;

namespace Feirapp.UnitTests.Helpers;

public class DbContextMock
{
    public static dynamic GetDbMock()
    {
        var client = new MongoClient("");
        var dataBase = client.GetDatabase(nameof(GroceryItem));
        return new Mock<IMongoCollection<object>>();
    }
}