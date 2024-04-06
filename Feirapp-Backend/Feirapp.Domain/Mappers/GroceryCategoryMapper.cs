using Feirapp.DocumentModels.Documents;
using Feirapp.Domain.Dtos;
using Riok.Mapperly.Abstractions;

namespace Feirapp.Domain.Mappers;

[Mapper]
public static partial class GroceryCategoryMapper
{
    public static partial GroceryCategoryDto ToDto(this GroceryCategory groceryCategory);

    public static partial List<GroceryCategoryDto> ToDtoList(this List<GroceryCategory> groceryCategory);

    public static partial GroceryCategory ToModel(this GroceryCategoryDto groceryCategoryDto);

    public static partial List<GroceryCategory> ToModelList(this List<GroceryCategoryDto> groceryCategory);
}