using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Domain.Class;
using Domain.DTO;
using Microsoft.IdentityModel.Tokens;

namespace API.BuilderFuctions;

internal static class JWTCreator
{
    internal static void SetupJWT(this WebApplicationBuilder builder)
    {
        string key = CreateJWTKey();
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "WIMSFP-sys",
                    ValidAudience = "WIMSFP-usr",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };
            });
    }
    
    
    
    static string CreateJWTKey()
    {
        byte[] keyBytes = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(keyBytes);
        }
        string secretKey = Convert.ToBase64String(keyBytes);
        Environment.SetEnvironmentVariable("JWT_SECRET_KEY", secretKey);
        return secretKey;
    }

    internal static string CreateToken(User user)
    {
        string key = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? CreateJWTKey();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: "WIMSFP-sys",
            audience: "WIMSFP-usr",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}