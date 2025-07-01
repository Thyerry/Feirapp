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
        "AC" => StatesEnum.AC,
        "AL" => StatesEnum.AL,
        "AP" => StatesEnum.AP,
        "AM" => StatesEnum.AM,
        "BA" => StatesEnum.BA,
        "CE" => StatesEnum.CE,
        "DF" => StatesEnum.DF,
        "ES" => StatesEnum.ES,
        "GO" => StatesEnum.GO,
        "MA" => StatesEnum.MA,
        "MT" => StatesEnum.MT,
        "MS" => StatesEnum.MS,
        "MG" => StatesEnum.MG,
        "PA" => StatesEnum.PA,
        "PB" => StatesEnum.PB,
        "PR" => StatesEnum.PR,
        "PE" => StatesEnum.PE,
        "PI" => StatesEnum.PI,
        "RJ" => StatesEnum.RJ,
        "RN" => StatesEnum.RN,
        "RS" => StatesEnum.RS,
        "RO" => StatesEnum.RO,
        "RR" => StatesEnum.RR,
        "SC" => StatesEnum.SC,
        "SP" => StatesEnum.SP,
        "SE" => StatesEnum.SE,
        "TO" => StatesEnum.TO,
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