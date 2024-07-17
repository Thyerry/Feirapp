using Feirapp.Domain.Services.GroceryItems.Command;
using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Domain.Services.GroceryItems.Responses;
using Feirapp.Entities.Entities;
using Feirapp.Entities.Enums;
using Riok.Mapperly.Abstractions;

namespace Feirapp.Domain.Services.GroceryItems.Mappers;

[Mapper]
public static partial class GroceryItemMapperly
{
    public static partial GroceryItemByStore ToStoreItem(this GroceryItem model);
    public static partial List<GroceryItemByStore> ToStoreItem(this List<GroceryItem> model);
    public static partial SearchGroceryItemsResponse ToSearchResponse(this SearchGroceryItemsDto model);
    public static partial List<SearchGroceryItemsResponse> ToSearchResponse(this List<SearchGroceryItemsDto> model);
    public static partial GroceryItem ToEntity(this InsertGroceryItem model);
    public static partial List<GroceryItem> ToEntity(this GroceryItemDto model);
    public static partial GetGroceryItemByIdResponse ToGetByIdResponse(this GroceryItem model);
    public static partial List<GetGroceryItemByIdResponse> ToGetByIdResponse(this List<GroceryItem> model);

    public static partial Store ToStore(this InsertStore model);

    private static List<string> SplitCommas(string nameList) => nameList.Split(",").ToList();

    private static StatesEnum ToStatesEnum(this string state) => state switch
    {
        "AC" => StatesEnum.Acre,
        "AL" => StatesEnum.Alagoas,
        "AP" => StatesEnum.Amapa,
        "AM" => StatesEnum.Amazonas,
        "BA" => StatesEnum.Bahia,
        "CE" => StatesEnum.Ceara,
        "DF" => StatesEnum.DistritoFederal,
        "ES" => StatesEnum.EspiritoSanto,
        "GO" => StatesEnum.Goias,
        "MA" => StatesEnum.Maranhao,
        "MT" => StatesEnum.MatoGrosso,
        "MS" => StatesEnum.MatoGrossoDoSul,
        "MG" => StatesEnum.MinasGerais,
        "PA" => StatesEnum.Parana,
        "PB" => StatesEnum.Paraiba,
        "PR" => StatesEnum.Parana,
        "PE" => StatesEnum.Pernambuco,
        "PI" => StatesEnum.Piaui,
        "RJ" => StatesEnum.RioDeJaneiro,
        "RN" => StatesEnum.RioGrandeDoNorte,
        "RS" => StatesEnum.RioGrandeDoSul,
        "RO" => StatesEnum.Rondonia,
        "RR" => StatesEnum.Roraima,
        "SC" => StatesEnum.SantaCatarina,
        "SP" => StatesEnum.SaoPaulo,
        "SE" => StatesEnum.Sergipe,
        "TO" => StatesEnum.Tocantins,
        _ => StatesEnum.Empty
    };

    private static MeasureUnitEnum ToMeasureUnitEnum(this string measureUnit) => measureUnit switch
    {
        "UN"  => MeasureUnitEnum.UNIT,
        "KG"  => MeasureUnitEnum.KILO,
        "L"   => MeasureUnitEnum.LITER,
        "M"   => MeasureUnitEnum.METER,
        "CX"  => MeasureUnitEnum.BOX,
        "PCE" => MeasureUnitEnum.PACKAGE,
        "CJ"  => MeasureUnitEnum.SET,
        _     => MeasureUnitEnum.EMPTY
    };
}