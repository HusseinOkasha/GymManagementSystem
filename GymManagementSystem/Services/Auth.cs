using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Gym_Management_System.Services;
using GymManagementSystem.Config;
using GymManagementSystem.Dtos;
using GymManagementSystem.Models;
using GymManagementSystem.Services.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GymManagementSystem.Services;

public class Auth : IAuth
{
    private readonly IOptions<JwtOption> _options;
    private readonly SignInManager<AccountModel> _signInManager;
    private readonly UserManager<AccountModel> _userManager;

    public Auth(IOptions<JwtOption> options,  SignInManager<AccountModel> signInManager,  UserManager<AccountModel> userManager)
    {
        _options = options;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public string GenerateToken(AccountModel user, IList<string> roles)
    {
        var secret = _options.Value.Secret;
        var issuer = _options.Value.ValidIssuer;
        var audiences = _options.Value.ValidationAudiences;
        if (secret == null || issuer == null || audiences == null)
            throw new ApplicationException("JWT is not set in the configuration ");
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var tokenHandler = new JwtSecurityTokenHandler();
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList());
        var tokenDescriptior = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
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
        var result = await tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = audiences,
            ValidIssuer = issuer,
            IssuerSigningKey = signingKey
        });
        return result.IsValid;
    }

    public async Task<string> Login(LoginDto dto)
    {
        // Find account with the given email
        AccountModel? account = await _userManager.FindByEmailAsync(dto.Email);
        if (account == null)
        {
            throw new UnauthorizedException("Invalid email or password");
        }
        
        // Try Signing in...
        SignInResult result = await _signInManager.PasswordSignInAsync(account, dto.Password, false, false);
        if (!result.Succeeded)
        {
            throw new UnauthorizedException("Invalid email or password");
        }
        
        // Get roles assigned to that account
        var roles = await _userManager.GetRolesAsync(account);
        
        // Generate access token 
        string token = GenerateToken(account, roles);
        return token;
    }
}