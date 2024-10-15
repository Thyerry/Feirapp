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
    [HttpGet("create-grocery-item")]
    [ProducesResponseType(typeof(InsertListOfGroceryItemsCommand), 200)]
    public async Task<IActionResult> CreateGroceryItem([FromQuery] CreateGroceryItemQuery query,
        CancellationToken ct)
    {
        var date = new Faker().Date.Past(1);
        var groceryItems = new Faker<InsertGroceryItem>()
            .CustomInstantiator(f => new InsertGroceryItem(
                f.Commerce.ProductName(),
                Math.Round(f.Random.Decimal(1, 100), 2),
                f.PickRandomWithout(MeasureUnitEnum.EMPTY).StringValue(),
                f.Commerce.Ean13(),
                null,
                null,
                date,
                f.Commerce.Ean8(),
                f.Commerce.Ean8()))
            .UseSeed(query.ProductSeed)
            .Generate(query.Quantity);

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
                State: f.PickRandom<StatesEnum>().StringValue()))
            .UseSeed(query.StoreSeed)
            .Generate();
        
        return Ok(new InsertListOfGroceryItemsCommand(groceryItems, store));
    }
}