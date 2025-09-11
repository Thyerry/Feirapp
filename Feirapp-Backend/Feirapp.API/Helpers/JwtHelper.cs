using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Feirapp.Domain.Services.Users.Methods.Login;
using Microsoft.IdentityModel.Tokens;
using static System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;


namespace Feirapp.API.Helpers;

public static class JwtHelper
{
    public static string GenerateJwtToken(LoginResponse user, IConfigurationSection jwtSettings)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        
        var claims = new []
        {
            new Claim(Sub, user.Email),
            new Claim(Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id) 
        };
        
        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.Now.Add(TimeSpan.Parse(jwtSettings["TokenLifetime"]!)),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}