using Bogus;
using Bogus.Extensions.Brazil;
using Feirapp.Domain.Services.GroceryItems.Command;
using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Domain.Services.GroceryItems.Queries;
using Feirapp.Entities.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Feirapp.API.Controllers;

[ApiController]
[Route("api/sandbox")]
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
        var date = new Faker().Date.Past(1);
        var groceryItems = new Faker<InsertGroceryItem>()
            .CustomInstantiator(f => new InsertGroceryItem(
                f.Commerce.ProductName(),
                Math.Round(f.Random.Decimal(1, 100), 2),
                f.PickRandomWithout(MeasureUnitEnum.EMPTY).GetStringValue(),
                f.Commerce.Ean13(),
                date,
                f.Commerce.Ean8(),
                f.Commerce.Ean8()))
            .UseSeed(payload.ProductSeed)
            .Generate(payload.Quantity);
        
        var store = new Faker<InsertStore>()
            .CustomInstantiator(f => new InsertStore(
                Name: f.Company.CompanyName(),
                AltNames: [f.Company.CompanyName(), f.Company.CompanyName()],
                Cnpj: f.Company.Cnpj().Replace(".", "").Replace("/", "").Replace("-", ""),
                Cep: f.Address.ZipCode(),
                Street: f.Address.StreetName(),
                StreetNumber: f.Address.BuildingNumber(),
                Neighborhood: f.Address.SecondaryAddress(),
                CityName: f.Address.City(),
                State: f.PickRandom<StatesEnum>().GetStringValue()))
            .UseSeed(payload.StoreSeed)
            .Generate();
                
        return Ok(new InsertListOfGroceryItemsCommand(groceryItems, store));
    }
}