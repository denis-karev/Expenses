using Expenses.Api.Common;
using Expenses.Api.Database;
using Expenses.Api.Models.Auth;
using Expenses.Api.Options;
using Expenses.Api.Services.Encryption;
using Microsoft.Extensions.Options;

namespace Expenses.Api.Services.Auth;

public sealed class AuthService(
    IDatabaseContext context,
    EncryptionService encryption,
    IOptions<EncryptionOptions> encryptionOptions,
    JwtTokenService tokens
)
{
    public async Task<InvokeResult<TokenResponse>> AuthenticateAsync(TokenRequest request)
    {
        return request.Method.ThrowIfNull() switch
        {
            EAuthMethod.Google => await new GoogleAuthStrategy(context, encryption, encryptionOptions, tokens)
                .AuthenticateAsync(request.Google.ThrowIfNull()),
            _ => throw new ArgumentOutOfRangeException(nameof(request))
        };
    }
}