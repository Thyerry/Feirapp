using Feirapp.Domain.Services.Users.Methods.CreateUser;
using Feirapp.Domain.Services.Users.Methods.Login;
using Feirapp.Entities.Entities;
using Riok.Mapperly.Abstractions;

namespace Feirapp.Domain.Mappers;

[Mapper]
public static partial class UserMappers
{
    public static partial User ToEntity(this CreateUserCommand command);
    public static partial LoginResponse ToLoginResponse(this User user);
}