using System.Security.Cryptography;
using System.Text;

namespace Feirapp.Domain.Services.Utils;

public static class PasswordHasher
{
    public static string GenerateSalt()
    {
        using var rng = RandomNumberGenerator.Create();
        var byteSalt = new byte[16];
        rng.GetBytes(byteSalt);
        var salt = Convert.ToBase64String(byteSalt);
        return salt;
    }
    
    public static string ComputeHash(string password, string salt)
    {
        using var sha256 = SHA256.Create();
        var passwordSaltPepper = $"{password}{salt}";
        var byteValue = Encoding.UTF8.GetBytes(passwordSaltPepper);
        var byteHash = sha256.ComputeHash(byteValue);
        return Convert.ToBase64String(byteHash);
    }
}