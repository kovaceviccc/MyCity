using System.IdentityModel.Tokens.Jwt;

namespace API.Extensions;

public static class TokenHelper
{
    public static string GetUserIdClaimFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == "Id")!.Value;
        return userId;
    }
}
