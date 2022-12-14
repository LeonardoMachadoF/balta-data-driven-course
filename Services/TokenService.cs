using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DataDriven.Models;
using Microsoft.IdentityModel.Tokens;

namespace DataDriven.Services;

public static class TokenService
{

    public static string GenerateToken(User user)
    {
        var key = Settings.GetKeyInBytes();

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]{
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            }),
            Expires = DateTime.UtcNow.AddHours(4),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}