namespace API.Extensions;

public class JwtConfig
{
    public string Secret { get; set; } = default!;
    public TimeSpan ExpiryTimeFrame { get; set; }
}
