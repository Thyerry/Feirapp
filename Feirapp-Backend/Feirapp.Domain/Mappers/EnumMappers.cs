using System.Diagnostics;
using Feirapp.Entities.Enums;

namespace Feirapp.Domain.Mappers;

public static class EnumMappers
{
    public static StatesEnum MapToStatesEnum(this string stateAbbreviation)
    {
        foreach (StatesEnum state in Enum.GetValues(typeof(StatesEnum)))
        {
            var fieldInfo = state.GetType().GetField(state.ToString());
            if (fieldInfo?.GetCustomAttributes(typeof(StringValueAttribute), false).FirstOrDefault() is StringValueAttribute attribute && attribute.Value == stateAbbreviation)
            {
                return state;
            }
        }

        return StatesEnum.Empty;
    }
    
    public static StatesEnum ToStatesEnum(string state) => state switch
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
        "PA" => StatesEnum.Para,
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
    
    public static MeasureUnitEnum ToMeasureUnitEnum(string measureUnit) => measureUnit switch
    {
        "UN" => MeasureUnitEnum.UNIT,
        "KG" => MeasureUnitEnum.KILO,
        "L" => MeasureUnitEnum.LITER,
        "M" => MeasureUnitEnum.METER,
        "CX" => MeasureUnitEnum.BOX,
        "PCE" => MeasureUnitEnum.PACKAGE,
        "PC" => MeasureUnitEnum.PACKAGE,
        "CJ" => MeasureUnitEnum.SET,
        "SC" => MeasureUnitEnum.SACK,
        _ => MeasureUnitEnum.EMPTY
    };
    
    public static string NormalizeMeasureUnit(this string value)
    {
        return value switch
        {
            "UN" => "UN",
            "UNID" => "UN",
            "KG" => "KG",
            "L" => "L",
            "M" => "M",
            "CX" => "CX",
            "PCE" => "PC",
            "PC" => "PC",
            "CJ" => "CJ",
            "SC" => "SC",
            _ => string.Empty
        };        
    }
}