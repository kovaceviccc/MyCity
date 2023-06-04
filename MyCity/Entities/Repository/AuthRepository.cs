using Entities.DbSet;
using Entities.Domain.Enums;
using Entities.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
namespace Entities.Repository;

public class AuthRepository : IAuthRepository
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly TokenValidationParameters _tokenValidationParameters;


    public AuthRepository(ApplicationDbContext contex,
                          UserManager<User> userManager,
                          IConfiguration configuration,
                          RoleManager<IdentityRole> roleManager,
                          TokenValidationParameters tokenValidationParameters)
    {
        _context = contex;
        _userManager = userManager;
        _configuration = configuration;
        _roleManager = roleManager;
        _tokenValidationParameters = tokenValidationParameters;
    }

    public async Task<User?> FindByEmailAsync(string email)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);    
            return user;
        }catch(Exception)
        {
            //log error
            return null;
        }
    }

    public async Task<User?> FindByIdAsync(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user;
        }
        catch (Exception)
        {
            //_logger.LogError($"User with id: {userId} ,not found");
            return null;
        }
    }

    public async Task<(string, string)> GenerateTokens(User user)
    {
        var token = await GenerateJwtToken(user); //returns tokenId and token(consider joining them in one method)

        var refreshToken = new RefreshToken
        {
            JwtId = token.Item1,
            Token = GenerateRefreshToken(),
            AddedDate = DateTime.UtcNow,
            ExpireDate = DateTime.UtcNow.AddMonths(6),
            UserId = user.Id,
        };

        user.RefreshTokens.Add(refreshToken);
        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();

        return (token.Item2, refreshToken.Token);
    }

    public async Task<(string, string)> GenerateTokens(string refreshToken)
    {
        try
        {
            var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(x=> x.Token.Equals(refreshToken));

            if(storedToken is null)
            {
                return new(string.Empty, string.Empty);
            }

            storedToken.IsUsed = true;
            _context.RefreshTokens.Update(storedToken);
            await _context.SaveChangesAsync();

            var user = await FindByIdAsync(storedToken.UserId);

            if(user is null)
            {
                return new(string.Empty, string.Empty);
            }
            
            return await GenerateTokens(user);

        }catch(Exception)
        {
            return new(string.Empty, string.Empty);
        }
    }

    public async Task<List<User>> GetAll()
    {
        return await _context.Users
            .Where(x=> x.IsBlocked == false)
            .ToListAsync();
    }

    public async Task<bool> RegisterUser(User user, string password)
    {
        try
        {
            var isCreated = await _userManager.CreateAsync(user, password);
            await _userManager.AddToRoleAsync(user, RolesEnum.User.ToString());
            
            return isCreated.Succeeded;
        }catch(Exception)
        {
            //log error
            return false;
        }

    }

    public async Task<bool> ValidatePassword(User user, string password)
    {
        try
        {
            var result = await _userManager.CheckPasswordAsync(user, password);
            return result;
        }catch(Exception) 
        {
            //log error
            return false;
        }
    }

    public async Task<bool> ValidateTokens(string jwtToken, string refreshToken)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();

        try
        {
            //ovo mora malo bolje
            _tokenValidationParameters.ValidateLifetime = false;
            var tokenInVerification = jwtTokenHandler.ValidateToken(jwtToken, _tokenValidationParameters, out var validatedToken);
            _tokenValidationParameters.ValidateLifetime = true;
            if(validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                if(result is false)
                {
                    return false;
                }
            }

            if(tokenInVerification is null)
            {
                return false;
            }

            var utcExpiryDate = long.Parse(tokenInVerification.Claims?.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp)!.Value!);

            var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

            if(expiryDate > DateTime.Now)
            {
                return false;
            }

            var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken);

            if(storedToken is null)
            {
                return false;
            }

            if(storedToken.IsUsed == true || storedToken.IsRevoked == true)
            {
                return false;
            }

            var jti = tokenInVerification.Claims?.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;

            if(storedToken.JwtId != jti)
            {
                return false;
            }

            if(storedToken.ExpireDate < DateTime.UtcNow)
            {
                return false;
            }


            return true;
        }
        catch (Exception)
        {
            //log error
            return false;
        }
    }

    private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
    {
        var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();
        return dateTimeVal;
    }

    private async Task<(string, string)> GenerateJwtToken(User user) 
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value!);

        var claims = await GetAllValidClaims(user);

        var tokenDecriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(TimeSpan.Parse(_configuration.GetSection("JwtConfig:ExpiryTimeFrame").Value!)),
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = jwtTokenHandler.CreateToken(tokenDecriptor);

        return (token.Id, jwtTokenHandler.WriteToken(token));
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private async Task<List<Claim>> GetAllValidClaims(User user)
    {
        var claims = new List<Claim>
        {
            new Claim("Id", user.Id),
            new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString())

        };

        //getting user claims
        var userClaims = await _userManager.GetClaimsAsync(user);
        claims.AddRange(userClaims);

        //get user roles and add it to the claims
        var userRoles = await _userManager.GetRolesAsync(user);

        foreach (var userRole in userRoles)
        {
            var role = await _roleManager.FindByNameAsync(userRole);

            if (role is not null)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var roleClaims = await _roleManager.GetClaimsAsync(role);
                foreach (var roleClaim in roleClaims)
                {
                    claims.Add(roleClaim);
                }
            }
        }
        return claims;
    }

    public async Task<bool> CreateRole(string roleName)
    {
        try
        {
            if(await _roleManager.RoleExistsAsync(roleName)) 
            {
                return false;
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));

            return result.Succeeded;

        }catch (Exception) 
        {
            return false;
        }
    }
}
