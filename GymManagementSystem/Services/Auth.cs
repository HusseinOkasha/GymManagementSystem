using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Gym_Management_System.Services;
using GymManagementSystem.Config;
using GymManagementSystem.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GymManagementSystem.Services;

public class Auth: IAuth
{
    public readonly IOptions<JwtOption> _options;

    public Auth(IOptions<JwtOption> options)
    {
        _options = options;
    }
    public string GenerateToken(AccountModel user)
    {
        var secret = _options.Value.Secret;
        var issuer = _options.Value.ValidIssuer;
        var audiences = _options.Value.ValidationAudiences;
        if (secret == null || issuer == null || audiences == null)
            throw new ApplicationException("JWT is not set in the configuration ");
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptior = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Email)
            }),
            Expires = DateTime.UtcNow.AddDays(1),
            Issuer = issuer,
            Audience = audiences,
            SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
        };
        var securityToken = tokenHandler.CreateToken(tokenDescriptior);
        var tokenString = tokenHandler.WriteToken(securityToken);
        return tokenString;
    }

    public async Task<bool> ValidateToken(string token)
    {
        var secret = _options.Value.Secret;
        var issuer = _options.Value.ValidIssuer;
        var audiences = _options.Value.ValidationAudiences;
        if (secret == null || issuer == null || audiences == null)
            throw new ApplicationException("JWT is not set in the configuration ");
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var tokenHandler = new JwtSecurityTokenHandler();
        var result = await tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = audiences,
            ValidIssuer = issuer,
            IssuerSigningKey = signingKey
        });
        return result.IsValid;

    }
}