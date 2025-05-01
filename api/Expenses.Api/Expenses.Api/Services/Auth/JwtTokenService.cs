using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Expenses.Api.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Expenses.Api.Services.Auth;

public sealed class JwtTokenService(IOptions<JwtOptions> jwtOptions)
{
    public JwtSecurityToken IssueAccessToken(Guid userId)
    {        
        Claim[] claims =
        [
            new(ClaimTypes.Name, userId.ToString()),
        ];

        return IssueToken(claims, jwtOptions.Value.Key, jwtOptions.Value.Issuer, jwtOptions.Value.Audience,
            DateTime.UtcNow.Add(jwtOptions.Value.AccessTokenLifetime));
    }

    public JwtSecurityToken IssueRefreshToken(Guid userId)
    {
        Claim[] claims =
        [
            new(ClaimTypes.Name, userId.ToString()),
        ];

        return IssueToken(claims, jwtOptions.Value.Key, jwtOptions.Value.Issuer, jwtOptions.Value.Audience,
            DateTime.UtcNow.Add(jwtOptions.Value.RefreshTokenLifetime));
    }
    
    private JwtSecurityToken IssueToken(Claim[] claims, Byte[] key, String issuer, String audience, DateTime expiresAt)
    {
        var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: creds);

        return token;
    }
}