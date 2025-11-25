using Bogus;
using Bogus.Extensions.Brazil;
using Feirapp.API.Helpers.Response;
using Feirapp.Domain.Services.GroceryItems.Methods.InsertGroceryItems;
using Feirapp.Entities.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Feirapp.API.Controllers;

public record CreateGroceryItemQuery(int Quantity = 1, int ProductSeed = 0, int StoreSeed = 0);

[ApiController]
[Route("api/sandbox")]
public class SandboxController : Controller
{
    [HttpGet("create-grocery-item")]
    [ProducesResponseType(typeof(ApiResponse<InsertGroceryItemsRequest>), 200)]
    public async Task<IActionResult> CreateGroceryItem([FromQuery] CreateGroceryItemQuery query, CancellationToken ct)
    {
        var date = new Faker().Date.Past(1);
        var groceryItems = new Faker<InsertGroceryItemsDto>()
            .CustomInstantiator(f => new InsertGroceryItemsDto
            {
                Name = f.Commerce.ProductName().ToUpper(),
                Price = Math.Round(f.Random.Decimal(1, 100), 2),
                MeasureUnit = f.PickRandomWithout(MeasureUnitEnum.EMPTY).StringValue(),
                Barcode = f.Commerce.Ean13(),
                ProductCode = f.Commerce.Ean8(),
                Brand = f.Company.CompanyName(),
                AltNames = f.Commerce.Categories(5).Select(c => c.ToUpper()).ToList(),
                PurchaseDate = date,
                CestCode = f.Commerce.Ean8(),
                NcmCode = f.Commerce.Ean8(),
                Description = f.Lorem.Paragraph(),
                ImageUrl = f.Image.PicsumUrl()
            })
            .UseSeed(query.ProductSeed)
            .Generate(query.Quantity);

        var store = new Faker<InsertGroceryItemsStoreDto>()
            .CustomInstantiator(f => new InsertGroceryItemsStoreDto
            {
                Name = f.Company.CompanyName().ToUpper(),
                AltNames = [f.Company.CompanyName(), f.Company.CompanyName()],
                Cnpj = f.Company.Cnpj().Replace(".", "").Replace("/", "").Replace("-", ""),
                Cep = f.Address.ZipCode(),
                Street = f.Address.StreetName(),
                StreetNumber = f.Address.BuildingNumber(),
                Neighborhood = f.Address.SecondaryAddress(),
                CityName = f.Address.City(),
                State = f.PickRandom<StatesEnum>().StringValue()
            })
            .UseSeed(query.StoreSeed)
            .Generate();

        return Ok(ApiResponseFactory.Success(new InsertGroceryItemsRequest
        {
            GroceryItems = groceryItems,
            Store = store
        }));
    }
}