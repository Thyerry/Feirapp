namespace Feirapp.Domain.Mappers;

public static class MapperUtils
{
    public static string StringAltNames(List<string> altNames) => string.Join("|", altNames);
    public static List<string> ListAltNames(string altNames) => altNames.Split("|").ToList();
}