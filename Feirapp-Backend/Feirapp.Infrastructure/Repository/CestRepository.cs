using Feirapp.Domain.Services.Cests.Interfaces;
using Feirapp.Entities.Entities;
using Feirapp.Infrastructure.Configuration;
using Feirapp.Infrastructure.Repository.BaseRepository;

namespace Feirapp.Infrastructure.Repository;

public class CestRepository(BaseContext context) : BaseRepository<Cest>(context), ICestRepository
{
    private readonly BaseContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task InsertListOfCodesAsync(List<string?> cestCodes, CancellationToken ct)
    {
        var validCestCodes = cestCodes
            .Where(code => !string.IsNullOrEmpty(code))
            .Distinct()
            .ToList();

        if (validCestCodes.Count == 0)
            return;

        var existingCestCodes = _context.Cests
            .Where(c => validCestCodes.Contains(c.Code))
            .Select(c => c.Code)
            .ToHashSet(); 

        var newCestCodes = validCestCodes
            .Where(code => !existingCestCodes.Contains(code))
            .Select(code => new Cest { Code = code })
            .ToList();

        if (newCestCodes.Any())
        {
            await _context.Cests.AddRangeAsync(newCestCodes, ct);
            await _context.SaveChangesAsync(ct);
        }
    }
}