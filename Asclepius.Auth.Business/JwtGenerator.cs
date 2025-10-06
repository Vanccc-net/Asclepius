using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Asclepius.Auth.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Asclepius.Auth.Business;

public class JwtGenerator(IConfiguration configuration)
{
    public string GenerateJwtToken(User user)
    {

        var claims = new List<Claim>
        {
            new(ClaimTypes.Sid, user.Id),
            new(ClaimTypes.Email, user.Email.Value)
        };

        foreach (var role in user.Roles) claims.Add(new Claim(ClaimTypes.Role, role.Name));

        var jwtSecret = configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key");

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            issuer: configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer"),
            audience: configuration["Jwt:Audience"]?? throw new ArgumentNullException("Jwt:Audience"),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)), 
                SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}