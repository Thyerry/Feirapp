using Feirapp.Domain.Services.DataScrapper.Dtos;
using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Entities.Entities;

namespace Feirapp.Domain.Mappers;

public static class StoreMappers
{
    public static StoreDto MapToDto(this Store store)
    {
        return new StoreDto
        (
            Id : store.Id,
            Name: store.Name,
            AltNames: store.AltNames,
            Cnpj: store.Cnpj,
            Cep: store.Cep,
            Street: store.Street,
            StreetNumber: store.StreetNumber,
            Neighborhood: store.Neighborhood,
            CityName: store.CityName,
            State: store.State);
    }
    
    public static List<StoreDto> MapToDto(this List<Store> stores)
    {
        return stores.Select(store => store.MapToDto()).ToList();
    }

    public static Store MapToEntity(this StoreDto storeDto)
    {
        return new Store
        {
            Name = storeDto.Name,
            Cnpj = storeDto.Cnpj,
            Cep = storeDto.Cep,
            Street = storeDto.Street,
            StreetNumber = storeDto.StreetNumber,
            Neighborhood = storeDto.Neighborhood,
            CityName = storeDto.CityName,
            State = storeDto.State
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
            State = store.State.MapToStatesEnum()
        };
    }
}