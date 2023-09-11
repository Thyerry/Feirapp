using Bogus;
using Feirapp.DocumentModels;
using Feirapp.Domain.Dtos;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;

namespace Feirapp.Tests.Fixtures;

public class GroceryItemFixture
{
    public static GroceryItemDto CreateRandomGroceryItemModel() => CreateGroceryItemsModels().FirstOrDefault()!;

    public static List<GroceryItemDto> CreateGroceryItemsModels(int quantity = 1)
    {
        var fakeGroceryItem = new Faker<GroceryItemDto>()
            .RuleFor(gi => gi.Id, ObjectId.GenerateNewId().ToString())
            .RuleFor(gi => gi.Name, f => f.Commerce.ProductName())
            .RuleFor(gi => gi.Price, f => (decimal)f.Random.Float() * 100)
            .RuleFor(gi => gi.Brand, f => f.Company.CompanyName())
            .RuleFor(gi => gi.StoreName, f => f.Company.CompanyName())
            .RuleFor(gi => gi.ImageUrl, f => f.Internet.Avatar())
            .RuleFor(gi => gi.PurchaseDate, FixtureUtils.FakeDate);

        return fakeGroceryItem.Generate(quantity);
    }

    public static GroceryItem CreateRandomGroceryItem() => CreateList().FirstOrDefault()!;

    public static List<GroceryItem> CreateList(int quantity = 1)
    {
        var fakePriceLog = new Faker<PriceLog>()
            .RuleFor(pl => pl.Price, f => (decimal)f.Random.Float() * 100)
            .RuleFor(pl => pl.LogDate, FixtureUtils.FakeDate);

        var fakeGroceryItem = new Faker<GroceryItem>()
            .RuleFor(gi => gi.Name, f => f.Commerce.ProductName())
            .RuleFor(gi => gi.Price, f => (decimal)f.Random.Float() * 100)
            .RuleFor(gi => gi.Brand, f => f.Company.CompanyName())
            .RuleFor(gi => gi.StoreName, f => f.Company.CompanyName())
            .RuleFor(gi => gi.ImageUrl, f => f.Internet.Avatar())
            .RuleFor(gi => gi.Category, GroceryCategoryFixture.CreateRandomGroceryCategory())
            .RuleFor(gi => gi.PriceHistory, fakePriceLog.Generate(3).ToList())
            .RuleFor(gi => gi.PurchaseDate, FixtureUtils.FakeDate)
            .RuleFor(gi => gi.Creation, FixtureUtils.FakeDate)
            .RuleFor(gi => gi.LastUpdate, FixtureUtils.FakeDate);

        return fakeGroceryItem.Generate(quantity);
    }
}