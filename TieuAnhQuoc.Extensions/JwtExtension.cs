using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TieuAnhQuoc.Extensions;

public static class JwtExtension
{
    private static readonly string Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? string.Empty;
    private static readonly string Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? string.Empty;
    private static readonly string Key = Environment.GetEnvironmentVariable("JWT_KEY") ?? string.Empty;

    public static string GenerateJwtToken(IEnumerable<Claim> claims, DateTime now, int expiresIn)
    {
        var key = Encoding.ASCII.GetBytes(Key);
        SymmetricSecurityKey securityKey = new(key);
        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256Signature);
        JwtHeader header = new(signingCredentials);
        var nowSeconds = now.TotalSeconds();
        JwtPayload payload = new()
        {
            {JwtRegisteredClaimNames.Iss, Issuer},
            {JwtRegisteredClaimNames.Aud, Audience},
            {JwtRegisteredClaimNames.Iat, nowSeconds},
            {JwtRegisteredClaimNames.Exp, nowSeconds + expiresIn}
        };
        payload.AddClaims(claims);
        JwtSecurityToken token = new(header, payload);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static string GenerateRefreshToken(int count = 64)
    {
        var bytes = RandomNumberGenerator.GetBytes(count);
        return Convert.ToBase64String(bytes);
    }

    public static IEnumerable<Claim> ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var keyByte = Encoding.ASCII.GetBytes(Key);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(keyByte),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = Issuer,
                ValidAudience = Audience,
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            var jwtToken = (JwtSecurityToken) validatedToken;
            return jwtToken.Claims;
        }
        catch
        {
            return new List<Claim>();
        }
    }
}