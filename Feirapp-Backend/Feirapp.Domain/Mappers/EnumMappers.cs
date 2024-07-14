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
            "L"  => MeasureUnitEnum.LITER,
            "M"  => MeasureUnitEnum.METER,
            _    => MeasureUnitEnum.UNIT
        };
    }

    public static string MapToString(this MeasureUnitEnum measureUnit)
    {
        return measureUnit switch
        {
            MeasureUnitEnum.UNIT => "UN",
            MeasureUnitEnum.KILO => "KG",
            MeasureUnitEnum.BOX  => "CX",
            MeasureUnitEnum.LITER => "L",
            MeasureUnitEnum.METER => "M",
            _ => "UN"
        };
    }
    
    public static StatesEnum MapToStatesEnum(this string stateAbbreviation)
    {
        foreach (StatesEnum state in Enum.GetValues(typeof(StatesEnum)))
        {
            var fieldInfo = state.GetType().GetField(state.ToString());
            var attribute = (StringValueAttribute)fieldInfo.GetCustomAttributes(typeof(StringValueAttribute), false).FirstOrDefault();
            if (attribute != null && attribute.Value == stateAbbreviation)
            {
                return state;
            }
        }
        return StatesEnum.Empty;
    }
}