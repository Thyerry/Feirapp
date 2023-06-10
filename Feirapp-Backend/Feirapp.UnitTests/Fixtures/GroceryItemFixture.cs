using Bogus;
using Feirapp.Domain.Enums;
using Feirapp.Domain.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Feirapp.UnitTests.Fixtures;

public class GroceryItemFixture
{
    public static List<GroceryItem> GetGroceryItems()
    {
        return new List<GroceryItem>
        {
            new()
            {
                Name = "Item 1",
                Price = 1.1,
                BrandName = "Brand 1",
                Id = new ObjectId().ToString(),
                GroceryCategory = GroceryCategoryEnum.DRINK,
                PurchaseDate = DateTime.Now,
                GroceryStoreName = "Store 1",
                PriceHistory = new List<PriceLog>(){new (){ Price= 0, LogDate = DateTime.Now}}
            },
            new()
            {
                Name = "Item 2",
                Price = 2.2,
                BrandName = "Brand 2",
                Id = new ObjectId().ToString(),
                GroceryCategory = GroceryCategoryEnum.MEAT,
                PurchaseDate = DateTime.Now,
                GroceryStoreName = "Store 2",
                PriceHistory = new List<PriceLog>(){new (){ Price= 2.2, LogDate = DateTime.Now}}
            },
            new()
            {
                Name = "Item 3",
                Price = 3.3,
                BrandName = "Brand 3",
                Id = new ObjectId().ToString(),
                GroceryCategory = GroceryCategoryEnum.CANNED,
                PurchaseDate = DateTime.Now,
                GroceryStoreName = "Store 3",
                PriceHistory = new List<PriceLog>(){new (){ Price= 3.3, LogDate = DateTime.Now}}
            }
        };
    }

    public static List<GroceryItem> GetGroceryItemsWithPriceHistory()
    {
        return new List<GroceryItem>
        {
            new()
            {
                Name = "Item 1",
                Price = 1.1,
                BrandName = "Brand 1",
                Id = new ObjectId().ToString(),
                GroceryCategory = GroceryCategoryEnum.DRINK,
                PurchaseDate = DateTime.Now,
                GroceryStoreName = "Store 1",
                PriceHistory = new List<PriceLog>(){new (){ Price= 1.1, LogDate = DateTime.Now}}
            },
            new()
            {
                Name = "Item 2",
                Price = 2.2,
                BrandName = "Brand 2",
                Id = new ObjectId().ToString(),
                GroceryCategory = GroceryCategoryEnum.MEAT,
                PurchaseDate = DateTime.Now,
                GroceryStoreName = "Store 2",
                PriceHistory = new List<PriceLog>(){new (){ Price= 2.2, LogDate = DateTime.Now}}
            },
            new()
            {
                Name = "Item 3",
                Price = 3.3,
                BrandName = "Brand 3",
                Id = new ObjectId().ToString(),
                GroceryCategory = GroceryCategoryEnum.CANNED,
                PurchaseDate = DateTime.Now,
                GroceryStoreName = "Store 3",
                PriceHistory = new List<PriceLog>(){new (){ Price= 3.3, LogDate = DateTime.Now}}
            }
        };
    }

    public static GroceryItem CreateRandomGroceryItem()
    {
        var dataSets = new MockDataSets();
        var fakePriceLog = new Faker<PriceLog>()
            .RuleFor(pl => pl.Price, f => f.Random.Float() * 100)
            .RuleFor(pl => pl.LogDate, f =>
            {
                var date = f.Date.Past();
                return new DateTime(date.Year, date.Month, date.Day);
            });

        var fakeGroceryItem = new Faker<GroceryItem>()
            .RuleFor(gi => gi.Name, f => f.Commerce.ProductName())
            .RuleFor(gi => gi.Price, f => f.Random.Float() * 100)
            .RuleFor(gi => gi.GroceryCategory, f => f.PickRandom<GroceryCategoryEnum>())
            .RuleFor(gi => gi.BrandName, f => f.Company.CompanyName())
            .RuleFor(gi => gi.GroceryStoreName, f => f.Company.CompanyName())
            .RuleFor(gi => gi.PurchaseDate, f =>
            {
                var date = f.Date.Past();
                return new DateTime(date.Year, date.Month, date.Day);
            })
            .RuleFor(gi => gi.GroceryImageUrl, f => f.Internet.Avatar())
            .RuleFor(gi => gi.PriceHistory, f => fakePriceLog.Generate(3).ToList());
            

        return fakeGroceryItem.Generate();
    }
}