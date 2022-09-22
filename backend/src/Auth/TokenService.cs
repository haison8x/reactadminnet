namespace Auth;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public interface ITokenService
{
    string BuildToken(string key, string issuer, UserModel user);

    bool IsTokenValid(string key, string issuer, string token);
}

public class TokenService : ITokenService
{
    private const double ExpiryDurationMinutes = 30;

    public string BuildToken(string key, string issuer, UserModel user)
    {
        var claims = new[] {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
        };

        var expireTime = DateTime.Now.AddMinutes(ExpiryDurationMinutes);
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(
            issuer,
            issuer,
            claims,
            expires: expireTime,
            signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    public bool IsTokenValid(string key, string issuer, string token)
    {
        var mySecret = Encoding.UTF8.GetBytes(key);
        var mySecurityKey = new SymmetricSecurityKey(mySecret);
        var tokenHandler = new JwtSecurityTokenHandler();
        var parameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = issuer,
            ValidAudience = issuer,
            IssuerSigningKey = mySecurityKey,
        };

        try
        {
            tokenHandler.ValidateToken(token, parameters, out _);
        }
        catch
        {
            return false;
        }

        return true;
    }
}