using Feirapp.Entities.Enums;

namespace Feirapp.Domain.Services.Stores.Methods.SearchStores;

public record SearchStoresRequest(string Name = "", StatesEnum State = StatesEnum.Empty, string CityName = "", int PageIndex = 0, int PageSize = 10);
