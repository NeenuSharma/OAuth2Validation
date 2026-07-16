using Identity.API.Repository;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;

namespace Identity.API.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ClaimsPrincipal?> ValidateUserAsync(
        string email,
        string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);

        if (user == null)
            return null;

        if (user.Password != password)
            return null;

        //var identity = new ClaimsIdentity(
        //    OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        //identity.AddClaim(OpenIddictConstants.Claims.Subject, user.Id.ToString());

        //identity.AddClaim(OpenIddictConstants.Claims.Email, user.Email);

        //identity.AddClaim(OpenIddictConstants.Claims.Name, user.Name);
        var identity = new ClaimsIdentity(
            OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);


        identity.AddClaim(
            OpenIddictConstants.Claims.Subject,
            user.Id.ToString(),
            OpenIddictConstants.Destinations.AccessToken);


        identity.AddClaim(
            OpenIddictConstants.Claims.Email,
            user.Email,
            OpenIddictConstants.Destinations.AccessToken);


        identity.AddClaim(
            OpenIddictConstants.Claims.Name,
            user.Name,
            OpenIddictConstants.Destinations.AccessToken);

        var principal = new ClaimsPrincipal(identity);

        principal.SetScopes(new[]
 {
    OpenIddictConstants.Scopes.OpenId,
    OpenIddictConstants.Scopes.Email,
    OpenIddictConstants.Scopes.Profile
});

        // Set the audience so the issued token's "aud" claim matches
        // the Audience expected by Product.API ("product-api").
        principal.SetResources("product-api");

        return principal;
    }
}