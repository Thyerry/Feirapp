using Feirapp.Domain.Services.Cests.Interfaces;
using Feirapp.Entities.Entities;
using Feirapp.Infrastructure.Configuration;
using Feirapp.Infrastructure.Extensions;

namespace Feirapp.Infrastructure.Repository;

public class CestRepository(BaseContext context) : ICestRepository
{
    public async Task InsertListOfCodesAsync(List<string?> cestCodes, CancellationToken ct)
    {
        var validCestCodes = cestCodes
            .Where(code => !string.IsNullOrEmpty(code))
            .Distinct()
            .ToList();

        if (validCestCodes.Count == 0)
            return;

        var existingCestCodes = context.Cests
            .Where(c => validCestCodes.Contains(c.Code))
            .Select(c => c.Code)
            .ToHashSet(); 

        var newCestCodes = validCestCodes
            .Where(code => !existingCestCodes.Contains(code))
            .Select(code => new Cest { Code = code })
            .ToList();

        if (newCestCodes.Any())
        {
            await context.Cests.AddRangeAsync(newCestCodes, ct);
        }
    }

    public async Task<Cest> AddIfNotExistsAsync(Func<Cest, bool> func, Cest cest, CancellationToken ct)
    {
        return await context.Cests.AddIfNotExistsAsync(cest, c => c.Code == cest.Code, ct);
    }
}