using Bogus;
using Bogus.Extensions.Brazil;
using Feirapp.Domain.Services.GroceryItems.Command;
using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Domain.Services.GroceryItems.Interfaces;
using Feirapp.Domain.Services.GroceryItems.Queries;
using Feirapp.Entities.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Feirapp.API.Controllers;

public class SandboxController : Controller
{
    public SandboxController()
    {
    }

    [HttpGet("create-grocery-item")]
    [ProducesResponseType(typeof(InsertListOfGroceryItemsCommand), 200)]
    public async Task<IActionResult> CreateGroceryItem([FromQuery] CreateGroceryItemPayload payload,
        CancellationToken ct)
    {
        var groceryItems = new Faker<InsertGroceryItem>()
            .CustomInstantiator(f => new InsertGroceryItem(
                f.Commerce.ProductName(),
                f.Random.Decimal(1, 100),
                f.PickRandom<MeasureUnitEnum>().ToString(),
                f.Commerce.Ean13(),
                f.Date.Past(1),
                f.Commerce.Ean8(),
                f.Commerce.Ean13())
            {
                Quantity = f.Random.Decimal(1, 100)
            })
            .UseSeed(payload.ProductSeed)
            .Generate(payload.Quantity);
        
        var store = new Faker<StoreDto>()
            .CustomInstantiator(f => new StoreDto(
                Id: null,
                Name: f.Company.CompanyName(),
                AltNames: [f.Company.CompanyName(2)],
                Cnpj: f.Company.Cnpj(),
                Cep: f.Address.ZipCode(),
                Street: f.Address.StreetName(),
                StreetNumber: f.Address.BuildingNumber(),
                Neighborhood: f.Address.SecondaryAddress(),
                CityName: f.Address.City(),
                State: f.PickRandom<StatesEnum>()))
            .UseSeed(payload.StoreSeed)
            .Generate();
                
        return Ok(new InsertListOfGroceryItemsCommand(groceryItems, store));
    }
}