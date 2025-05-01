using System.IdentityModel.Tokens.Jwt;
using Expenses.Api.Common;
using Expenses.Api.Database;
using Expenses.Api.Entities;
using Expenses.Api.Model;
using Expenses.Api.Models.Auth;
using Expenses.Api.Options;
using Expenses.Api.Services.Encryption;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Oauth2.v2;
using Google.Apis.Oauth2.v2.Data;
using Google.Apis.Services;
using Microsoft.Extensions.Options;

namespace Expenses.Api.Services.Auth;

public sealed class GoogleAuthStrategy(
    IDatabaseContext context,
    EncryptionService encryption,
    IOptions<EncryptionOptions> encryptionOptions,
    JwtTokenService tokens
) : IAuthStrategy<GoogleAuthRequest>
{
    public async Task<InvokeResult<TokenResponse>> AuthenticateAsync(GoogleAuthRequest request)
    {
        var credentials = GoogleCredential.FromAccessToken(request.Token);
        var oauth2Service = new Oauth2Service(new BaseClientService.Initializer
        {
            HttpClientInitializer = credentials
        });
        var userInfo = await oauth2Service.Userinfo.Get().ExecuteAsync();

        if (userInfo is null)
            return InvokeResult<TokenResponse>.CreateError(EErrorType.BadRequest, "Invalid token.");
        if (userInfo.Email.IsNullOrEmpty())
            return InvokeResult<TokenResponse>.CreateError(EErrorType.BadRequest, "Cannot access user email. Does the token have the right scopes?");

        var user = await User.FindByEmailAsync(context, userInfo.Email);
        if (user is null)
            user = await RegisterAsync(userInfo, request.Token);
        else
            await UpdateUserTokenAsync(user, request.Token);

        var accessToken = tokens.IssueAccessToken(user.Info.Id);
        var result = new TokenResponse(
            AccessToken: new JwtSecurityTokenHandler().WriteToken(accessToken),
            Expiration: accessToken.ValidTo
        );
        
        return InvokeResult<TokenResponse>.CreateSuccess(result);
    }

    private async Task<User> RegisterAsync(Userinfo userInfo, String token)
    {
        var encryptedToken = encryption.Encrypt(token, encryptionOptions.Value.GoogleKeyBytes);
        var info = new UserInfo(
            Id: Guid.NewGuid(),
            Email: userInfo.Email,
            Name: userInfo.Name,
            EncryptedGoogleToken: encryptedToken
        );
        return await User.CreateAsync(context, info);
    }

    private async Task UpdateUserTokenAsync(User user, String token)
    {
        var encryptedToken = encryption.Encrypt(token, encryptionOptions.Value.GoogleKeyBytes);
        var info = user.Info with
        {
            EncryptedGoogleToken = encryptedToken
        };
        await user.UpdateAsync(info); 
    }
}