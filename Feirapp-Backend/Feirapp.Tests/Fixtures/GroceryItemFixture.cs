using Bogus;
using Feirapp.Domain.Models;
using Feirapp.Entities;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Feirapp.Tests.Fixtures;

public class GroceryItemFixture
{
    public static List<GroceryItemModel> GetGroceryItems()
    {
        return new List<GroceryItemModel>
        {
            new()
            {
                Name = "Item 1",
                Price = 1.1m,
                Brand = "Brand 1",
                Id = new ObjectId().ToString(),
                PurchaseDate = DateTime.Now,
                GroceryStore = "Store 1",
                PriceHistory = new List<PriceLogModel>(){new (){ Price = 0, LogDate = DateTime.Now}}
            },
            new()
            {
                Name = "Item 2",
                Price = 2.2m,
                Brand = "Brand 2",
                Id = new ObjectId().ToString(),
                PurchaseDate = DateTime.Now,
                GroceryStore = "Store 2",
                PriceHistory = new List<PriceLogModel>(){new (){ Price= 2.2m, LogDate = DateTime.Now}}
            },
            new()
            {
                Name = "Item 3",
                Price = 3.3m,
                Brand = "Brand 3",
                Id = new ObjectId().ToString(),
                PurchaseDate = DateTime.Now,
                GroceryStore = "Store 3",
                PriceHistory = new List<PriceLogModel>(){new (){ Price= 3.3m, LogDate = DateTime.Now}}
            }
        };
    }

    public static GroceryItem CreateRandomGroceryItem() => CreateList().FirstOrDefault()!;

    public static List<GroceryItem> CreateList(int quantity = 1)
    {
        var fakeGroceryItem = new Faker<GroceryItem>()
            .RuleFor(gi => gi.Name, f => f.Commerce.ProductName())
            .RuleFor(gi => gi.Price, f => (decimal)f.Random.Float() * 100)
            .RuleFor(gi => gi.Brand, f => f.Company.CompanyName())
            .RuleFor(gi => gi.GroceryStore, f => f.Company.CompanyName())
            .RuleFor(gi => gi.ImageUrl, f => f.Internet.Avatar())
            .RuleFor(gi => gi.PurchaseDate, f =>
            {
                var date = f.Date.PastDateOnly();
                return new DateTime(date.Year, date.Month, date.Day).ToUniversalTime();
            });

        return fakeGroceryItem.Generate(quantity);
    }
}