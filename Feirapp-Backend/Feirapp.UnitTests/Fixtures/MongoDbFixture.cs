using Feirapp.DAL.DataContext;

namespace Feirapp.UnitTests.Fixtures
{
    public class MongoDbFixture
    {
        public MongoFeirappContext Context { get; set; }

        public MongoDbFixture()
        {
            var mongoSettings = new MongoSettings()
            {
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = "Feirapp",
            };
            Context = new MongoFeirappContext(new OptionsConfigurationMock<MongoSettings>(mongoSettings));
        }
    }
}