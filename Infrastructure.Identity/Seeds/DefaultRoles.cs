using Application.Enums;

using Infrastructure.Identity.Models;

using Microsoft.AspNetCore.Identity;

using System.Threading.Tasks;

namespace Infrastructure.Identity.Seeds;

public static class DefaultRoles
{
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        //Seed Roles
        _ = await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
        _ = await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
        _ = await roleManager.CreateAsync(new IdentityRole(Roles.Moderator.ToString()));
        _ = await roleManager.CreateAsync(new IdentityRole(Roles.Basic.ToString()));
    }
}