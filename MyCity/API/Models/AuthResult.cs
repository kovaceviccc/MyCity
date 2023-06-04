using System.Net;

namespace API.Models;

public class AuthResult
{
    public string Token { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
    public bool Result { get; set; }
    public List<string> Errors { get; set; } = new();
}
