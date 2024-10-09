using Feirapp.Domain.Services.Users.Interfaces;
using Feirapp.Entities.Entities;
using Feirapp.Infrastructure.Configuration;
using Feirapp.Infrastructure.Repository.BaseRepository;

namespace Feirapp.Infrastructure.Repository;

public class UserRepository(BaseContext context) : BaseRepository<User>(context), IUserRepository
{
}