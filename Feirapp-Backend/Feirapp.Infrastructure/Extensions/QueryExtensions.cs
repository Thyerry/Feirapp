using Microsoft.EntityFrameworkCore;

namespace Feirapp.Infrastructure.Extensions;

public static class QueryExtensions
{
    public static bool ILike(this string column, string value) => EF.Functions.ILike(EF.Functions.Unaccent(column), EF.Functions.Unaccent($"%{value}%"));
    public static bool StartsWith(this string column, string value) => EF.Functions.ILike(EF.Functions.Unaccent(column), EF.Functions.Unaccent($"{value}%"));
    public static bool EndsWith(this string column, string value) => EF.Functions.ILike(EF.Functions.Unaccent(column), EF.Functions.Unaccent($"%{value}"));
}