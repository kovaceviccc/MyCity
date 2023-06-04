using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Entities.DbSet;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace API.Extensions;

public static class Auth
{
    //public static bool ValidatePassword(this User user, string password)
    //{
    //    return BCrypt.Net.BCrypt.HashPassword(password, user.PasswordSalt) == user.PasswordHash;
    //}

    //public static (string, string) HashPassword(string password)
    //{
    //    var salt = BCrypt.Net.BCrypt.GenerateSalt();
    //    var passwordHash = BCrypt.Net.BCrypt.HashPassword(password, salt);
    //    return (salt, passwordHash);
    //}

    //public static (string, string) GenerateTokens(this User user, string secret)
    //{
    //    return (GenerateJwtToken(user, DateTime.Now.AddMinutes(20), secret), GenerateRefreshToken());
    //}

    //private static string GenerateJwtToken(User user,
    //    DateTime expirationTime,
    //    string secret)
    //{
    //    var key = Encoding.ASCII.GetBytes(secret);
    //    var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
    //    var claims = new List<Claim>
    //    {
    //        new("Id", user.Id.ToString())
    //    };
    //    claims.AddRange(user.Roles.Select(role => new Claim("Roles", role.ToString())));
    //    var token = new JwtSecurityToken(
    //        claims: claims,
    //        expires: expirationTime,
    //        signingCredentials: credentials);

    //    return new JwtSecurityTokenHandler().WriteToken(token);

    //    //return "jwtToken";
    //}

    //private async static GenerateJwtToken(User user, string secret)
    //{
    //    var jwtTokenHandler = new JwtSecurityTokenHandler();
    //    var key = Encoding.UTF8.GetBytes(secret);
    //    var roles =

    //    //token decriptor
    //    var tokenDecriptor = new SecurityTokenDescriptor
    //    {
    //        Subject = new ClaimsIdentity(new[]
    //        {
    //            new Claim("Id", user.Id),
    //            new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
    //            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
    //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    //            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
    //            new Claim("Roles", string.Join(",", roles))

    //        }),
    //        Expires = DateTime.UtcNow.Add(TimeSpan.Parse(_configuration.GetSection("JwtConfig:ExpiryTimeFrame").Value!)),
    //        SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
    //    };

    //    var token = jwtTokenHandler.CreateToken(tokenDecriptor);
    //    var jwtToken = jwtTokenHandler.WriteToken(token);

    //    var refreshToken = new RefreshToken
    //    {
    //        JwtId = token.Id,
    //        Token = jwtToken,
    //        AddedDate = DateTime.UtcNow,
    //        ExpireDate = DateTime.UtcNow.AddMonths(6),
    //        UserId = user.Id,
    //    };

    //    await _context.RefreshTokens.AddAsync(refreshToken);
    //    await _context.SaveChangesAsync();

    //    return jwtToken;
    //}



    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
