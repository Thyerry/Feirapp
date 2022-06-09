using System;
using System.Collections.Generic;
using Feirapp.Domain.Enums;
using Feirapp.Domain.Models;

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
                ID = Guid.NewGuid(),
                ProductSection = ProductSectionEnum.DRINKS,
                PurchaseDate = DateTime.Now,
                GroceryStoreName = "Store 1"
            },
            new()
            {
                Name = "Item 2",
                Price = 2.2,
                BrandName = "Brand 2",
                ID = new Guid(),
                ProductSection = ProductSectionEnum.MEATS,
                PurchaseDate = DateTime.Now,
                GroceryStoreName = "Store 2"
            },
            new()
            {
                Name = "Item 3",
                Price = 3.3,
                BrandName = "Brand 3",
                ID = new Guid(),
                ProductSection = ProductSectionEnum.BOTTLED,
                PurchaseDate = DateTime.Now,
                GroceryStoreName = "Store 3"
            }
        };
    }
}