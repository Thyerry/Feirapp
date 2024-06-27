using Feirapp.Entities.Enums;

namespace Feirapp.Domain.Mappers;

public static class EnumMappers
{
    public static MeasureUnitEnum MapToMeasureUnit(this string measureUnit)
    {
        return measureUnit switch
        {
            "UN" => MeasureUnitEnum.UNIT,
            "KG" => MeasureUnitEnum.KILO,
            "CX" => MeasureUnitEnum.BOX,
            "L" => MeasureUnitEnum.LITER,
            "M" => MeasureUnitEnum.METER,
            _ => MeasureUnitEnum.UNIT
        };
    }
    
    public static string MapToString(this MeasureUnitEnum measureUnit)
    {
        return measureUnit switch
        {
            MeasureUnitEnum.UNIT => "UN",
            MeasureUnitEnum.KILO => "KG",
            MeasureUnitEnum.BOX => "CX",
            MeasureUnitEnum.LITER => "L",
            MeasureUnitEnum.METER => "M",
            _ => "UN"
        };
    }
    
    public static StatesEnum MapToStatesEnum(this string state)
    {
        return state switch
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
            _ => StatesEnum.EMPTY
        };
    }
}