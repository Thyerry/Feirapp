using Feirapp.Domain.Services.Ncms.Interfaces;
using Feirapp.Entities.Entities;
using Feirapp.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Feirapp.Infrastructure.Repository;

public class NcmRepository(BaseContext context) : INcmRepository
{
    public async Task<List<Ncm>> GetByQuery(Func<Ncm, bool> func, CancellationToken ct)
    {
        return await context.Ncms
            .Where(func)
            .AsQueryable()
            .ToListAsync(ct);
    }

    public new async Task UpdateAsync(Ncm entity, CancellationToken ct)
    {
        var ncm = await GetByCodeAsync(entity.Code!, ct);
        ncm.Description = entity.Description;
        ncm.LastUpdate = DateTime.Now.Date;
        context.Ncms.Update(ncm);
    }

    public async Task<Ncm?> GetByCodeAsync(string code, CancellationToken ct)
    {
        return await context.Ncms
            .FirstOrDefaultAsync(x => x.Code == code, ct);
    }

    public async Task InsertListOfCodesAsync(List<string> ncmCodes, CancellationToken ct)
    {
        var validNcms = ncmCodes
            .Where(code => !string.IsNullOrEmpty(code))
            .Distinct() 
            .ToList();

        if (validNcms.Count == 0)
            return;

        var existingNcms = context.Ncms
            .Where(c => validNcms.Contains(c.Code))
            .Select(c => c.Code)
            .ToHashSet();

        var newNcms = validNcms
            .Where(code => !existingNcms.Contains(code))
            .Select(code => new Ncm { Code = code })
            .ToList();

        if (newNcms.Count != 0)
        {
            await context.Ncms.AddRangeAsync(newNcms, ct);
        }
    }
}