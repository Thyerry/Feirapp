using System.ComponentModel.DataAnnotations.Schema;
using Feirapp.Entities.Enums;

namespace Feirapp.Entities.Entities;

[Table("users")]
public class User
{
    [Column("id")]
    public Guid Id { get; set; }
    [Column("name")]
    public string Name { get; set; }
    [Column("email")]
    public string Email { get; set; }
    [Column("password")]
    public string Password { get; set; }
    [Column("password_salt")]
    public string PasswordSalt { get; set; }
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    [Column("last_login")]
    public DateTime LastLogin { get; set; }
    [Column("failed_login_attempts")]    
    public int FailedLoginAttempts { get; set; } = 0;
    [Column("status'")]
    public UserStatus Status { get; set; }
}