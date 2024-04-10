using Feirapp.Domain.Services.DataScrapper.Dtos;
using Feirapp.Entities.Entities;

namespace Feirapp.Domain.Services.DataScrapper.Mappers;

public static class InvoiceGroceryItemMapper
{
    public static List<GroceryItem> MapToEntity(this List<InvoiceGroceryItem> groceryItems, InvoiceStore store)
    {
        return groceryItems.Select(x => x.MapToEntity(store)).ToList();
    }
    
    public static GroceryItem MapToEntity(this InvoiceGroceryItem groceryItem, InvoiceStore store)
    {
        return new GroceryItem
        {
            Name = groceryItem.Name,
            Price = groceryItem.Price,
            Barcode = groceryItem.Barcode,
            NcmCode = groceryItem.Ncm,
            CestCode = groceryItem.Cest,
            Store = store.MapToEntity()
        };
    }
    
    public static Store MapToEntity(this InvoiceStore store)
    {
        return new Store
        {
            Name = store.Name,
            Cnpj = store.Cnpj,
            Cep = store.Cep,
            Street = store.Street,
            StreetNumber = store.StreetNumber,
            Neighborhood = store.Neighborhood,
            CityName = store.CityName,
        };
    }
}