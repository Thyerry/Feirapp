using System.Runtime.Intrinsics.X86;
using Feirapp.Domain.Services.GroceryItems.Interfaces;
using Feirapp.Entities.Entities;
using Feirapp.Infrastructure.Configuration;
using Feirapp.Infrastructure.Repository.BaseRepository;
using Microsoft.EntityFrameworkCore;

namespace Feirapp.Infrastructure.Repository;

public class GroceryItemRepository(BaseContext context)
    : BaseRepository<GroceryItem>(context), IGroceryItemRepository, IDisposable
{
    private readonly BaseContext _context = context;

   

    public void Dispose()
    {
    }
}