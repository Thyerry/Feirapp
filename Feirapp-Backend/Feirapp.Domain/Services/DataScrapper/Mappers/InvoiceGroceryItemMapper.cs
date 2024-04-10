using Feirapp.Domain.Services.DataScrapper.Dtos;
using Feirapp.Domain.Services.GroceryItems.Dtos.Commands;
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
    
    public static List<InsertGroceryItemCommand> MapToInsertCommand(this List<InvoiceGroceryItem> groceryItems, InvoiceStore store)
    {
        return groceryItems.Select(x => x.MapToInsertCommand(store)).ToList();
    }
    
    public static InsertGroceryItemCommand MapToInsertCommand(this InvoiceGroceryItem groceryItem, InvoiceStore store)
    {
        return new InsertGroceryItemCommand
        {
            Name = groceryItem.Name,
            Price = groceryItem.Price,
            Barcode = groceryItem.Barcode,
            NcmCode = groceryItem.Ncm,
            CestCode = groceryItem.Cest,
            StoreCep = store.Cep,
            StoreCityName = store.CityName,
            StoreCnpj = store.Cnpj,
            StoreName = store.Name,
            StoreNeighborhood = store.Neighborhood,
        };
    }
}