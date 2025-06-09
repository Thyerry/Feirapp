using Feirapp.Domain.Services.Cests.Interfaces;
using Feirapp.Domain.Services.GroceryItems.Interfaces;
using Feirapp.Domain.Services.Ncms.Interfaces;
using Feirapp.Domain.Services.Stores.Interfaces;
using Feirapp.Domain.Services.UnitOfWork;
using Feirapp.Domain.Services.Users.Interfaces;
using Feirapp.Infrastructure.Repository;

namespace Feirapp.Infrastructure.Configuration;

public class UnitOfWork(BaseContext context) : IUnitOfWork, IDisposable
{
    #region Repositories Interfaces

    private IGroceryItemRepository? _groceryItemRepository;
    private IStoreRepository? _storeRepository;
    private INcmRepository? _ncmRepository;
    private ICestRepository? _cestRepository;
    private IUserRepository? _userRepository;

    #endregion

    #region Repositories Instances

    public IGroceryItemRepository GroceryItemRepository => _groceryItemRepository ??= new GroceryItemRepository(context);
    public IStoreRepository StoreRepository => _storeRepository ??= new StoreRepository(context);
    public INcmRepository NcmRepository => _ncmRepository ??= new NcmRepository(context);
    public ICestRepository CestRepository => _cestRepository ??= new CestRepository(context);
    public IUserRepository UserRepository => _userRepository ??= new UserRepository(context);

    #endregion

    public async Task<int> SaveChangesAsync(CancellationToken ct)
    {
        return await context.SaveChangesAsync(ct);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}