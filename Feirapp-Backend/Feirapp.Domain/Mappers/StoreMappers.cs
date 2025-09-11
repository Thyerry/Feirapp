using Feirapp.Domain.Services.GroceryItems.Methods.GetGroceryItemsByStore;
using Feirapp.Domain.Services.GroceryItems.Methods.InsertListOfGroceryItems;
using Feirapp.Domain.Services.Stores.Methods.GetStoreById;
using Feirapp.Domain.Services.Stores.Methods.InsertGroceryItem;
using Feirapp.Entities.Entities;
using Feirapp.Entities.Enums;
using Riok.Mapperly.Abstractions;

namespace Feirapp.Domain.Mappers;

[Mapper]
public static partial class StoreMappers
{
    public static partial GetGroceryItemsByStoreStoreDto ToResponse(this Store entity);
    public static partial GetStoreByIdResponse ToGetStoreByIdResponse(this Store entity);
    public static partial Store ToEntity(this InsertListOfGroceryItemsStoreDto entity);
    public static partial Store ToEntity(this InsertStoreRequest entity);
    private static StatesEnum ToStatesEnum(this string state) => EnumMappers.ToStatesEnum(state);
}