using System;
using System.Collections.Generic;
using Feirapp.Domain.Enums;
using Feirapp.Domain.Models;
using MongoDB.Bson;

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
                GroceryStoreName = "Store 1"
            },
            new()
            {
                Name = "Item 2",
                Price = 2.2,
                BrandName = "Brand 2",
                Id = new ObjectId().ToString(),
                GroceryCategory = GroceryCategoryEnum.MEAT,
                PurchaseDate = DateTime.Now,
                GroceryStoreName = "Store 2"
            },
            new()
            {
                Name = "Item 3",
                Price = 3.3,
                BrandName = "Brand 3",
                Id = new ObjectId().ToString(),
                GroceryCategory = GroceryCategoryEnum.CANNED,
                PurchaseDate = DateTime.Now,
                GroceryStoreName = "Store 3"
            }
        };
    }
}