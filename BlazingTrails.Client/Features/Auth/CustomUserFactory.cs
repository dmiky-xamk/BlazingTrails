using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using System.Security.Claims;
using System.Text.Json;

namespace BlazingTrails.Client.Features.Auth;

// Auth0 returns roles in an array and we have to seperate them in order to make Blazor understand them.
public class CustomUserFactory<TAccount> :
    AccountClaimsPrincipalFactory<RemoteUserAccount>
{
    public CustomUserFactory(IAccessTokenProviderAccessor accessor) 
        : base(accessor) { }

    // Override so that custom logic can be applied when a user is authenticated / logs in into the application.
    public override async ValueTask<ClaimsPrincipal> CreateUserAsync(RemoteUserAccount account, RemoteAuthenticationUserOptions options)
    {
        // Creates 'ClaimsPrincipal' representing the user based on the token sent back by Auth0.
        var initialUser = await base.CreateUserAsync(account, options);

        if (initialUser?.Identity?.IsAuthenticated ?? false)
        {
            var userIdentity = (ClaimsIdentity)initialUser.Identity;

            // Contains the claims for the user and their values in JSON format.
            account.AdditionalProperties.TryGetValue(ClaimTypes.Role, out var roleClaimValue);

            // Check that we actually have an array of roles.
            if (roleClaimValue is not null
                && roleClaimValue is JsonElement element
                && element.ValueKind == JsonValueKind.Array)
            {
                // Remove the original role array.
                userIdentity.RemoveClaim(userIdentity.FindFirst(ClaimTypes.Role));

                // Generate a single role claim for each role and add them to 'ClaimsIdentity' representing the current user.
                var claims = element.EnumerateArray().Select(x => new Claim(ClaimTypes.Role, x.ToString()));

                userIdentity.AddClaims(claims);
            }
        }

        return initialUser ?? new ClaimsPrincipal();
    }
}
