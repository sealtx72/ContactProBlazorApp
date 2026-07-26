using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ContactProBlazorApp.Data
{
    public class DataUtility
    {
        public static async Task ManageDataAsync(IServiceProvider svcProvider)
        {
            var dbContextSrc = svcProvider.GetRequiredService<ApplicationDbContext>();
            var userManagerSvc = svcProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var config = svcProvider.GetRequiredService<IConfiguration>();

            await dbContextSrc.Database.MigrateAsync();
            await SeedDemoUserAsync(userManagerSvc, config);
        }

        public static async Task SeedDemoUserAsync(UserManager<ApplicationUser> userManager, IConfiguration config) 
        {
            try
            {
                string? demoEmail = config["DemoUserLogin"];
                string? demoPassword = config["DemoUserPassword"];

                if (string.IsNullOrEmpty(demoEmail) || string.IsNullOrEmpty(demoPassword))
                {
                    throw new Exception("Demo user credentials are not configured.");
                }

                ApplicationUser demoUser = new()
                {
                    UserName = demoEmail,
                    Email = demoEmail,
                    FirstName = "Demo",
                    LastName = "Login",
                    EmailConfirmed = true,
                };

                ApplicationUser? user = await userManager.FindByEmailAsync(demoUser.Email);

                if (user == null)
                {
                    await userManager.CreateAsync(demoUser, demoPassword);
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error seeding demo user: {ex.Message}"); 
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
