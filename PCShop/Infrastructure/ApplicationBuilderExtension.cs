using Microsoft.AspNetCore.Identity;
using PCShop.Entities;

namespace PCShop.Infrastructure
{
    public static class ApplicationBuilderExtension
    {
        public static async Task<IApplicationBuilder> PrepareDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var services = serviceScope.ServiceProvider;

            await RoleSeeder(services);
            await SeedAdministrator(services);


            return app;
        }

        private static async Task RoleSeeder(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { "Client", "Employee" };

            IdentityResult roleResult;

            foreach (var role in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(role);

                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }


        private static async Task SeedAdministrator(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            if (await userManager.FindByNameAsync("admin") == null)
            {
                var user = new User();
                user.UserName = "admin";
                user.Email = "admin@admin.com";
                user.FirstName = "admin";
                user.LastName = "admin";
                user.IsAdministrator = true;
                user.RegisterDate = DateTime.UtcNow;

                var result = await userManager.CreateAsync
                (user, "123123");

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Employee").Wait();
                }
            }
        }

    }
}
