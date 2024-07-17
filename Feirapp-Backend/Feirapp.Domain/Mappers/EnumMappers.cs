using Feirapp.Entities.Enums;

namespace Feirapp.Domain.Mappers;

public static class EnumMappers
{
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