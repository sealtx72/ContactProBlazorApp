using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Bogus;
using ContactProBlazorApp.Models;
using ContactProBlazorApp.Client.Models.Enums;
using System.Reflection;

namespace ContactProBlazorApp.Data
{
    public class DataUtility
    {
        public static async Task ManageDataAsync(IServiceProvider svcProvider)
        {
            var dbContextSvc = svcProvider.GetRequiredService<ApplicationDbContext>();
            var userManagerSvc = svcProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var config = svcProvider.GetRequiredService<IConfiguration>();

            await dbContextSvc.Database.MigrateAsync();
            await SeedDemoUserAsync(userManagerSvc, config);
            await SeedDemoContactsAsync(userManagerSvc, dbContextSvc, config);
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

        public static async Task SeedDemoContactsAsync(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IConfiguration config)
        {
            string? demoEmail = config["DemoUserLogin"];
            if (string.IsNullOrEmpty(demoEmail)) return;

            var user = await userManager.FindByEmailAsync(demoEmail);
            if (user == null)
            {
                return;
            }

            var demoContacts = await context.Contacts
                .Where(c => c.AppUserId == user.Id)
                .Include(c => c.Categories)
                .ToListAsync();

            var demoCategories = await context.Categories
                .Where(c => c.AppUserId == user.Id)
                .ToListAsync();

            Random rand = new();

            if (demoContacts.Count == 0)
            {
                var newContacts = new Faker<Contact>()
                    .RuleFor(c => c.LastName, f => f.Name.LastName())
                    .RuleFor(c => c.Birthdate, f => f.Date.Between(
                                DateTime.Now - TimeSpan.FromDays(365 * 60),
                                DateTime.Now - TimeSpan.FromDays(365 * 18)
                                ))
                    .RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumber())
                    .RuleFor(c => c.Address1, f => f.Address.StreetAddress())
                    .RuleFor(c => c.City, f => f.Address.City())
                    .RuleFor(c => c.State, f => f.PickRandom<State>())
                    .RuleFor(c => c.ZipCode, f => int.Parse(f.Address.ZipCode("#####")))
                    .RuleFor(c => c.AppUserId, user.Id)
                    .Generate(10);

                Faker faker = new();

                var imageDir = Path.Combine(Directory.GetCurrentDirectory(), "Data/DemoImages/");
                var mensPics = Directory.GetFiles(Path.Combine(imageDir, "Men/")).ToList();
                var womensPics = Directory.GetFiles(Path.Combine(imageDir, "Women/")).ToList();

                for (int i = 0; i < newContacts.Count; i++)
                {
                    Contact contact = newContacts[i];

                    if (i % 2 == 0)
                    {
                        contact.FirstName = faker.Name.FirstName(Bogus.DataSets.Name.Gender.Male);
                        if (mensPics.Count > 0)
                        {
                            var pic = mensPics[rand.Next(0, mensPics.Count)];
                            mensPics.Remove(pic);

                            ImageUpload image = new()
                            {
                                Data = await File.ReadAllBytesAsync(pic),
                                Type = $"image/{Path.GetExtension(pic).TrimStart('.')}"
                            };

                            contact.Image = image;
                            context.Images.Add(image);
                        }
                    }
                    else
                    {
                        contact.FirstName = faker.Name.FirstName(Bogus.DataSets.Name.Gender.Female);
                        if (womensPics.Count > 0)
                        {
                            var pic = womensPics[rand.Next(0, womensPics.Count)];
                            womensPics.Remove(pic);
                            ImageUpload image = new()
                            {
                                Data = await File.ReadAllBytesAsync(pic),
                                Type = $"image/{Path.GetExtension(pic).TrimStart('.')}"
                            };
                            contact.Image = image;
                            context.Images.Add(image);
                        }
                    }

                    contact.Email = faker.Internet.Email(contact.FirstName, contact.LastName, "mailinator.com");
                    if (rand.Next() % 2 == 0)
                    {
                        contact.Address2 = new Faker().Address.SecondaryAddress();
                    }

                    contact.Created = new Faker().Date.Between(DateTime.Now.AddYears(-1), DateTime.Now);
                }

                demoContacts.AddRange(newContacts);
            }

            if (demoCategories.Count == 0)
            {
                demoCategories = [
                    new() { Name= "Family", AppUserId = user.Id },
                    new() { Name= "Friends", AppUserId = user.Id },
                    new() { Name= "CoWorkers", AppUserId = user.Id },
                    new() { Name= "Clients", AppUserId = user.Id },
                    new() { Name= "School", AppUserId = user.Id },
                    new() { Name= "Gaming", AppUserId = user.Id },
                    new() { Name= "Favorites", AppUserId = user.Id },
                    ];

                context.Categories.AddRange(demoCategories);
            }

            foreach (var contact in demoContacts.Where(c => c.Categories?.Count == 0))
            {
                int numCategories = rand.Next(1, 5);
                var categories = demoCategories
                    .OrderBy(c => Guid.NewGuid())
                    .Take(numCategories);

                contact.Categories = [.. categories];
                context.Update(contact);

            }

            await context.SaveChangesAsync();

        }
    }
}
