using Feirapp.Domain.Services.BaseRepository;
using Feirapp.Domain.Services.Ncms.Interfaces;
using Feirapp.Entities.Entities;
using Feirapp.Infrastructure.Configuration;
using Feirapp.Infrastructure.Repository.BaseRepository;
using Microsoft.EntityFrameworkCore;

namespace Feirapp.Infrastructure.Repository;

public class NcmRepository : BaseRepository<Ncm>, INcmRepository 
{
    private readonly BaseContext _context;
    public NcmRepository(BaseContext context) : base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
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
}