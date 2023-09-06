using Feirapp.Infrastructure.DataContext;

namespace Feirapp.Tests.Helpers;

public class MongoDbContextMock
{
    public MongoFeirappContext Context { get; set; }

    public MongoDbContextMock()
    {
        var mongoSettings = new MongoSettings()
        {
            ConnectionString = "mongodb://localhost:27017",
            DatabaseName = "Feirapp",
        };
        Context = new MongoFeirappContext(new OptionsConfigurationMock<MongoSettings>(mongoSettings));
    }
}