using ContactProBlazorApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace ContactProBlazorApp.Components.Account
{
    public class CustomUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, IOptions<IdentityOptions> options)
                                                                : UserClaimsPrincipalFactory<ApplicationUser>(userManager, options)

    {
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            ClaimsIdentity identity = await base.GenerateClaimsAsync(user);

            List<Claim> customClaims =
                [
                    new Claim("FirstName", user.FirstName!),
                    new Claim("LastName", user.LastName!)
                ];

            identity.AddClaims(customClaims);
            return identity;
        }
    }
}
