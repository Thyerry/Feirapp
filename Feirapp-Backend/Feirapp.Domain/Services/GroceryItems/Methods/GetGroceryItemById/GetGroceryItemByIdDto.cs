using Feirapp.Entities.Enums;

namespace Feirapp.Domain.Services.GroceryItems.Methods.GetGroceryItemById;

public class GetGroceryItemByIdDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? Brand { get; set; }
    public string Barcode { get; set; }
    public string NcmCode { get; set; }
    public string CestCode { get; set; }
    public MeasureUnitEnum MeasureUnit { get; set; }
    public List<GetGroceryItemByIdPriceLogDto> PriceHistory { get; set; }

    public GetGroceryItemByIdResponse ToResponse()
    {
        return new GetGroceryItemByIdResponse(Id, Name, Description, ImageUrl, Brand, Barcode, NcmCode, CestCode, MeasureUnit, PriceHistory);
    }
}