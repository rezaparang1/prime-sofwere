using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Prime_Software;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    public TokenService(IConfiguration config) => _config = config;

    public TokenResponse GenerateToken(BusinessEntity.Settings.User user)
    {
        var jwt = _config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // شناسه کاربر
            new Claim(ClaimTypes.Name, user.UserName ?? ""),
            new Claim("GroupUserId", user.GroupUserId.ToString())
        };

        if (user.Group_User?.AccessLevel != null)
        {
            claims.Add(new Claim("AccessLevelId", user.Group_User.AccessLevel.Id.ToString()));
        }

        var expires = DateTime.UtcNow.AddMinutes(int.Parse(jwt["ExpireMinutes"]!));

        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

        return new TokenResponse
        {
            Token = tokenStr,
            ExpiresAt = expires
        };
    }
}

