using System.Text;

namespace Expenses.Api.Options;

public sealed record JwtOptions
{
    public required String SecretKey { get; init; }
    public required String RefreshTokenSecret { get; init; }
    public required String Issuer { get; init; }
    public required String Audience { get; init; }

    public Byte[] Key => Encoding.UTF8.GetBytes(SecretKey);
}