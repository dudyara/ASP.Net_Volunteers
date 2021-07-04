namespace Volunteers.DB
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Volunteers.Entities;

    /// <summary>
    /// Initializer
    /// </summary>
    public class Initializer
    {
        /// <summary>
        /// InitializeAsync
        /// </summary>
        /// <param name="userManager">userManager</param>
        /// <param name="roleManager">roleManager</param>
        /// <returns></returns>
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            string adminEmail = "admin@mail.ru";
            string password = "Avk21!";
            if (await roleManager.FindByNameAsync("Admin") == null)
            {
                await roleManager.CreateAsync(new Role { Name = "Admin" });
            }

            if (await roleManager.FindByNameAsync("Organization") == null)
            {
                await roleManager.CreateAsync(new Role { Name = "Organization" });
            }

            if (await userManager.FindByEmailAsync("adminEmail") == null)
            {
                User admin = new User { Id = 1, Email = adminEmail, UserName = adminEmail, RoleId = 1 };
                await userManager.CreateAsync(admin, password);
            }
        }
    }
}
