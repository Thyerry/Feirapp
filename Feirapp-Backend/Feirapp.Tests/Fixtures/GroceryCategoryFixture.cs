using Bogus;
using Feirapp.DocumentModels.Documents;
using System.Collections.Generic;
using System.Linq;

namespace Feirapp.Tests.Fixtures
{
    public class GroceryCategoryFixture
    {
        public static GroceryCategory CreateRandomGroceryCategory() => CreateListGroceryCategory().FirstOrDefault()!;

        public static List<GroceryCategory> CreateListGroceryCategory(int quantity = 1)
        {
            var fakeGroceryItem = new Faker<GroceryCategory>()
                .RuleFor(gi => gi.Name, f => f.Commerce.ProductName())
                .RuleFor(gi => gi.Cest, f => $"{f.Random.Number(1, 9_999_999):0000000}")
                .RuleFor(gi => gi.Ncm, f => $"{f.Random.Number(1, 99_999_999):00000000}")
                .RuleFor(gi => gi.ItemNumber, f => $"{f.Random.Float() * 100:00.00}")
                .RuleFor(gi => gi.Description, f => f.Lorem.Paragraph());

            return fakeGroceryItem.Generate(quantity);
        }
    }
}