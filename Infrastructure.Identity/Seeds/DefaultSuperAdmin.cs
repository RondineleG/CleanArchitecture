using Application.Enums;

using Infrastructure.Identity.Models;

using Microsoft.AspNetCore.Identity;

using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Seeds;

public static class DefaultSuperAdmin
{
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        //Seed Default User
        ApplicationUser defaultUser = new()
        {
            UserName = "superadmin",
            Email = "superadmin@gmail.com",
            FirstName = "Mukesh",
            LastName = "Murugan",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };
        if (userManager.Users.All(u => u.Id != defaultUser.Id))
        {
            ApplicationUser user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
            {
                _ = await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                _ = await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
                _ = await userManager.AddToRoleAsync(defaultUser, Roles.Moderator.ToString());
                _ = await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                _ = await userManager.AddToRoleAsync(defaultUser, Roles.SuperAdmin.ToString());
            }

        }
    }
}
