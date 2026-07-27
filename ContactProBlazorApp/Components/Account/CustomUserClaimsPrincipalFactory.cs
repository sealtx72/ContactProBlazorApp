using ContactProBlazorApp.Client.Models;
using ContactProBlazorApp.Data;
using ContactProBlazorApp.Helpers;
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

            string profilePictureUrl = user.ProfilePictureId.HasValue ? $"/uploads/{user.ProfilePictureId}" : ImageHelper.DefaultProfilePictureUrl;

            List<Claim> customClaims =
            [
                new Claim(nameof(UserInfo.ProfilePictureUrl), profilePictureUrl),
                new Claim("FirstName", user.FirstName!),
                new Claim("LastName", user.LastName!)
            ];

            identity.AddClaims(customClaims);

            return identity;
        }
    }
}
