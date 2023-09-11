﻿using Feirapp.Domain.Dtos;
using Feirapp.DocumentModels;
using Riok.Mapperly.Abstractions;

namespace Feirapp.Domain.Mappers;

[Mapper]
public static partial class GroceryCategoryMapper
{
    public static partial GroceryCategoryDto ToDto(this GroceryCategory groceryCategory);

    public static partial List<GroceryCategoryDto> ToDtoList(this List<GroceryCategory> groceryCategory);

    public static partial GroceryCategory ToModel(this GroceryCategoryDto groceryCategoryDto);
}