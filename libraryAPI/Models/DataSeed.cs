using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace libraryAPI.Models
{
    public class DataSeed
    {
        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                IdentityRole studentRole = new IdentityRole();
                studentRole.Name = "Admin";
                IdentityResult result = roleManager.CreateAsync(studentRole).Result;
            }
            if (!roleManager.RoleExistsAsync("User").Result)
            {
                IdentityRole parentRole = new IdentityRole();
                parentRole.Name = "User";
                IdentityResult result = roleManager.CreateAsync(parentRole).Result;
            }
        }

        public static void SeedAdmin(UserManager<IdentityUser> userManager)
        {
            if (userManager.FindByEmailAsync("admin").Result == null)
            {
                IdentityUser adminUser = new IdentityUser();
                adminUser.UserName = "admin@admin.pl";
                adminUser.Email = "admin@admin.pl";

                IdentityResult result = userManager.CreateAsync(adminUser, "Admin!23").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(adminUser, "Admin").Wait();
                }
            }
        }
    }
}
