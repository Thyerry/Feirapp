using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Feirapp.Infrastructure.Extensions;

public class UtcDateTimeConverter() : ValueConverter<DateTime, DateTime>(
    convertToProviderExpression: v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
    v => DateTime.SpecifyKind(v, DateTimeKind.Utc));