namespace Feirapp.Entities.Enums;

public class StringValueAttribute(string value) : Attribute
{
    public string Value { get; private set; } = value;
}

public static class StringValueAttributeExtensions
{
    public static string GetStringValue(this Enum value)
    {
        var type = value.GetType();
        var fieldInfo = type.GetField(value.ToString());
        var attributes = fieldInfo.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
        return attributes?.Length > 0 ? attributes[0].Value : null;
    }
}