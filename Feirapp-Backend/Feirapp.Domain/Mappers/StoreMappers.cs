using Feirapp.Domain.Services.GroceryItems.Methods.GetGroceryItemsByStore;
using Feirapp.Domain.Services.GroceryItems.Methods.InsertGroceryItems;
using Feirapp.Domain.Services.Stores.Methods.GetStoreById;
using Feirapp.Domain.Services.Stores.Methods.InsertGroceryItem;
using Feirapp.Domain.Services.Stores.Methods.SearchStores;
using Feirapp.Entities.Entities;
using Feirapp.Entities.Enums;
using Riok.Mapperly.Abstractions;

namespace Feirapp.Domain.Mappers;

[Mapper]
public static partial class StoreMappers
{
    public static partial List<SearchStoresResponse> ToSearchResponse(this List<Store> store);
    public static partial GetGroceryItemsByStoreStoreDto ToResponse(this Store entity);
    public static partial GetStoreByIdResponse ToGetStoreByIdResponse(this Store entity);
    public static partial Store ToEntity(this InsertGroceryItemsStoreDto entity);
    public static partial Store ToEntity(this InsertStoreRequest entity);
    private static StatesEnum ToStatesEnum(this string state) => EnumMappers.ToStatesEnum(state);
}