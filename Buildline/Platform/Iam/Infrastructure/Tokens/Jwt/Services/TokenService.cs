using System.Security.Claims;
using System.Text;
using Buildline.Platform.Iam.Application.Internal.OutboundServices;
using Buildline.Platform.Iam.Domain.Model.Aggregates;
using Buildline.Platform.Iam.Infrastructure.Tokens.Jwt.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Buildline.Platform.Iam.Infrastructure.Tokens.Jwt.Services;

/// <summary>
///     JWT implementation of IAM token generation and validation.
/// </summary>
/// <param name="tokenSettings">Options containing the symmetric token signing secret.</param>
/// <remarks>
///     Generated tokens include the user id, email, display name and role claims required by the
///     ASP.NET Core JWT Bearer middleware and the frontend session state.
/// </remarks>
public class TokenService(IOptions<TokenSettings> tokenSettings) : ITokenService
{
    private readonly TokenSettings _tokenSettings = tokenSettings.Value;

    /// <inheritdoc />
    public string GenerateToken(User user)
    {
        var key = Encoding.ASCII.GetBytes(_tokenSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var tokenHandler = new JsonWebTokenHandler();
        return tokenHandler.CreateToken(tokenDescriptor);
    }

    /// <inheritdoc />
    public async Task<int?> ValidateToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token)) return null;

        var tokenHandler = new JsonWebTokenHandler();
        var key = Encoding.ASCII.GetBytes(_tokenSettings.Secret);

        try
        {
            var result = await tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            });

            var jwtToken = (JsonWebToken)result.SecurityToken;
            return int.Parse(jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value);
        }
        catch
        {
            return null;
        }
    }
}
