using System.Text;

namespace Expenses.Api.Options;

public sealed record EncryptionOptions
{
    public required String GoogleKey { get; init; }
    
    public Byte[] GoogleKeyBytes => Convert.FromBase64String(GoogleKey);
}