using Feirapp.Domain.Models;
using Feirapp.Entities;
using Riok.Mapperly.Abstractions;

namespace Feirapp.Domain.Mappers;

[Mapper]
public static partial class GroceryCategoryMapper
{
    public static partial GroceryCategoryModel ToModel(this GroceryCategory groceryCategory);
    public static partial GroceryCategory ToEntity(this GroceryCategoryModel groceryCategoryModel);
}