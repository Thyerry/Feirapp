using Bogus;
using Bogus.DataSets;

namespace Feirapp.UnitTests.Fixtures
{
    public class MockDataSets
    {
        public Address Address { get; set; }
        public Commerce Commerce { get; set; }
        public Company Company { get; set; }
        public Date Date { get; set; }
        public Finance Finance { get; set; }
        public Internet Internet { get; set; }
        public Randomizer Randomizer { get; set; }

        public MockDataSets()
        {
            Address = new Address();
            Commerce = new Commerce();
            Company = new Company();
            Date = new Date();
            Finance = new Finance();
            Internet = new Internet();
            Randomizer = new Randomizer();
        }
    }
}