using Feirapp.DAL.DataContext;

namespace Feirapp.UnitTests.Helpers;

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