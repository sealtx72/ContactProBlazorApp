using ContactProBlazorApp.Client.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace ContactProBlazorApp.Client.Helpers
{
    public static class UserInfoHelper
    {
        public static UserInfo? GetUserInfo(AuthenticationState authState)
        {
            var userId = authState.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var email = authState.User.FindFirst(ClaimTypes.Email)!.Value;
            var firstName = authState.User.FindFirst("FirstName")!.Value;
            var lastName = authState.User.FindFirst("LastName")!.Value;
            string? profilePictureUrl = authState.User.FindFirst(nameof(UserInfo.ProfilePictureUrl))?.Value;

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(firstName) || 
                string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(profilePictureUrl))
            {
                return null;
            }

            UserInfo userInfo = new UserInfo
            {
                UserId = userId,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                ProfilePictureUrl = profilePictureUrl
            };

            return userInfo;
        }

        public static async Task<UserInfo?> GetUserInfoAsync(Task<AuthenticationState>? authStateTask)
        {
            if (authStateTask == null)
            {
                return null;  
            }
            else
            {
                AuthenticationState authState = await authStateTask;
                UserInfo? userInfo = GetUserInfo(authState);
                return userInfo;
            }
        }
    }
}
