using Feirapp.Domain.Services.Cests.Interfaces;
using Feirapp.Entities.Entities;
using Feirapp.Infrastructure.Configuration;
using Feirapp.Infrastructure.Repository.BaseRepository;

namespace Feirapp.Infrastructure.Repository;

public class CestRepository(BaseContext context) : BaseRepository<Cest>(context), ICestRepository
{
    private readonly BaseContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public Task InsertListOfCodesAsync(List<string>? cestCodes, CancellationToken ct)
    {
        var cests = cestCodes.Select(x => new Cest { Code = x }).ToList();
        return _context.Cests.BulkInsertAsync(cests, options =>
        {
            options.InsertIfNotExists = true;
            options.ColumnPrimaryKeyExpression = c => c.Code;
        }, ct);
    }
}