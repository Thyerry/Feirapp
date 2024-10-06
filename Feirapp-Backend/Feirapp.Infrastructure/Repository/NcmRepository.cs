using Feirapp.Domain.Services.Ncms.Interfaces;
using Feirapp.Entities.Entities;
using Feirapp.Infrastructure.Configuration;
using Feirapp.Infrastructure.Repository.BaseRepository;
using Microsoft.EntityFrameworkCore;

namespace Feirapp.Infrastructure.Repository;

public class NcmRepository(BaseContext context) : BaseRepository<Ncm>(context), INcmRepository
{
    private readonly BaseContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public new async Task UpdateAsync(Ncm entity, CancellationToken ct)
    {
        var ncm = await GetByCodeAsync(entity.Code!, ct);
        ncm.Description = entity.Description;
        ncm.LastUpdate = DateTime.Now.Date;
        await base.UpdateAsync(ncm, ct);
    }

    public async Task<Ncm?> GetByCodeAsync(string code, CancellationToken ct)
    {
        return await _context.Ncms
            .FirstOrDefaultAsync(x => x.Code == code, ct);
    }

    public async Task InsertListOfCodesAsync(List<string> ncmCodes, CancellationToken ct)
    {
        var validNcms = ncmCodes
            .Where(code => !string.IsNullOrEmpty(code))
            .Distinct() 
            .ToList();

        if (!validNcms.Any())
            return;

        var existingNcms = _context.Cests
            .Where(c => validNcms.Contains(c.Code))
            .Select(c => c.Code)
            .ToHashSet();

        var newNcms = validNcms
            .Where(code => !existingNcms.Contains(code))
            .Select(code => new Ncm { Code = code })
            .ToList();

        if (newNcms.Any())
        {
            await _context.Ncms.AddRangeAsync(newNcms, ct);
            await _context.SaveChangesAsync(ct);
        }
    }
}