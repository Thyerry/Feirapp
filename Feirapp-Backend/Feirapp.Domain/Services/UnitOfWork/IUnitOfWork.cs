using Feirapp.Domain.Services.Cests.Interfaces;
using Feirapp.Domain.Services.GroceryItems.Interfaces;
using Feirapp.Domain.Services.Ncms.Interfaces;
using Feirapp.Domain.Services.Stores.Interfaces;
using Feirapp.Domain.Services.Users.Interfaces;

namespace Feirapp.Domain.Services.UnitOfWork;

public interface IUnitOfWork
{
    public IGroceryItemRepository  GroceryItemRepository { get; }
    public IStoreRepository StoreRepository { get; }
    public INcmRepository NcmRepository { get; }
    public ICestRepository CestRepository { get; }
    public IUserRepository UserRepository { get; }
    public Task<int> SaveChangesAsync(CancellationToken ct);
}