using Feirapp.Domain.Mappers;
using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Domain.Services.Stores.Dtos.Commands;
using Feirapp.Domain.Services.Stores.Interfaces;
using Feirapp.Entities.Entities;

namespace Feirapp.Domain.Services.Stores.Implementations;

public class StoreService(IStoreRepository storeRepository) : IStoreService
{
    private readonly IStoreRepository _storeRepository = storeRepository;

    public async Task InsertStoreAsync(InsertStoreCommand store, CancellationToken ct)
    {
        var storeEntity = new Store
        {
            Name = store.Name,
            AltNames = store.AltNames,
            Cnpj = store.Cnpj,
            Cep = store.Cep,
            Street = store.Street,
            StreetNumber = store.StreetNumber,
            Neighborhood = store.Neighborhood,
            CityName = store.CityName,
            State = store.State?.MapToStatesEnum()
        };
        await _storeRepository.InsertAsync(storeEntity, ct);
    }

    public async Task<List<StoreDto>?> GetAllStoresAsync(CancellationToken ct)
    {
        var result = await _storeRepository.GetAllAsync(ct);
        return result.MapToDto();
    }

    public async Task<StoreDto?> GetStoreById(long storeId, CancellationToken ct)
    {
        var result = await _storeRepository.GetByIdAsync(storeId, ct);
        return result?.MapToDto();
    }
}