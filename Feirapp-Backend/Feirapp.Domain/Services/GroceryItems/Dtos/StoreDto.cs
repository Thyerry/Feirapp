using Feirapp.Entities.Enums;
using FluentValidation.Results;

namespace Feirapp.Domain.Services.GroceryItems.Dtos;

public record StoreDto(
    long? Id,
    string Name,
    List<string>? AltNames,
    string? Cnpj = null,
    string? Cep = null,
    string? Street = null,
    string? StreetNumber = null,
    string? Neighborhood = null,
    string? CityName = null,
    StatesEnum? State = StatesEnum.Empty)
{
    public List<ValidationFailure> ValidateForInsertGroceryItem()
    {
        var errors = new List<ValidationFailure>();
        if (string.IsNullOrWhiteSpace(Name))
            errors.Add(new ValidationFailure(nameof(Name), "Name cannot be null or empty.", Name));
        if (string.IsNullOrWhiteSpace(Cnpj))
            errors.Add(new ValidationFailure(nameof(Cnpj), "Cnpj cannot be null or empty.", Cnpj));
        if (string.IsNullOrWhiteSpace(Cep))
            errors.Add(new ValidationFailure(nameof(Cep), "Cep cannot be null or empty.", Cep));
        if (string.IsNullOrWhiteSpace(Street))
            errors.Add(new ValidationFailure(nameof(Street), "Street cannot be null or empty.", Street));
        if (string.IsNullOrWhiteSpace(StreetNumber))
            errors.Add(new ValidationFailure(nameof(StreetNumber), "Street Number cannot be null or empty.", StreetNumber));
        if (string.IsNullOrWhiteSpace(Neighborhood))
            errors.Add(new ValidationFailure(nameof(Neighborhood), "Neighborhood cannot be null or empty.", Neighborhood));
        if (string.IsNullOrWhiteSpace(CityName))
            errors.Add(new ValidationFailure(nameof(CityName), "City Name cannot be null or empty.", CityName));
        if (State == null)
            errors.Add(new ValidationFailure(nameof(State), "State cannot be null.", State.ToString()));

        return errors;
    }    
}