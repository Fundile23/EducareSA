using Microsoft.AspNetCore.Identity;

namespace EducareSA.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAdminAsync(
            IServiceProvider serviceProvider)
        {
            var roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var userManager =
                serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            const string adminRole = "Admin";

            const string adminEmail = "admin@educaresa.co.za";

            const string adminPassword = "Admin@12345";

            // Create Admin role if it doesn't exist
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(
                    new IdentityRole(adminRole));
            }

            // Find admin user
            var adminUser =
                await userManager.FindByEmailAsync(adminEmail);

            // Create admin user
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result =
                    await userManager.CreateAsync(
                        adminUser,
                        adminPassword);

                if (!result.Succeeded)
                {
                    throw new Exception(
                        string.Join(
                            ", ",
                            result.Errors.Select(e => e.Description)));
                }
            }

            // Add user to Admin role
            if (!await userManager.IsInRoleAsync(
                    adminUser,
                    adminRole))
            {
                await userManager.AddToRoleAsync(
                    adminUser,
                    adminRole);
            }
        }
    }
}