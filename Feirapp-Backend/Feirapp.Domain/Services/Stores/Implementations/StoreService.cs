using Feirapp.Domain.Mappers;
using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Domain.Services.Stores.Dtos.Commands;
using Feirapp.Domain.Services.Stores.Interfaces;
using Feirapp.Entities.Entities;

namespace Feirapp.Domain.Services.Stores.Implementations;

public class StoreService(IStoreRepository storeRepository) : IStoreService
{
    public async Task InsertStoreAsync(InsertStoreCommand store, CancellationToken ct)
    {
        var storeEntity = new Store
        {
            Name = store.Name,
            AltNames = string.Join(",", store.AltNames),
            Cnpj = store.Cnpj,
            Cep = store.Cep,
            Street = store.Street,
            StreetNumber = store.StreetNumber,
            Neighborhood = store.Neighborhood,
            CityName = store.CityName,
            State = store.State.MapToStatesEnum()
        };
        await storeRepository.InsertAsync(storeEntity, ct);
    }

    public async Task<List<StoreDto>?> GetAllStoresAsync(CancellationToken ct)
    {
        var result = await storeRepository.GetAllAsync(ct);
        return result.ToDtoList();
    }

    public async Task<StoreDto?> GetStoreById(long storeId, CancellationToken ct)
    {
        var result = await storeRepository.GetByIdAsync(storeId, ct);
        return result?.ToDto();
    }
}