using Feirapp.Entities.Enums;

namespace Feirapp.Entities.Entities;

public class User
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PasswordSalt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastLogin { get; set; }
    public int FailedLoginAttempts { get; set; } = 0;
    public UserStatus Status { get; set; }
}