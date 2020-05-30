using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace libraryAPI.Entities
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

        public static void SeedAdmin(UserManager<User> userManager)
        {
            if (userManager.FindByEmailAsync("admin").Result == null)
            {
                User adminUser = new User();
                adminUser.UserName = "admin@admin.pl";
                adminUser.Email = "admin@admin.pl";

                IdentityResult result = userManager.CreateAsync(adminUser, "Admin!23").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(adminUser, "Admin").Wait();
                }
            }
        }
        public static void Seed3Books(LibraryDbContext _context)
        {
            if(!_context.Books.Any(b => b.Title == "Dzieci Z Bulerbyn"))
            {
                var book1 = new Book
                {
                    IsAvailable = true,
                    Title = "Dzieci z Bulerbyn",
                    WhoTookIt = null,
                    Year = 1886,
                    BookAuthors = null
                };
                _context.Books.Add(book1);
            }

            if (!_context.Books.Any(b => b.Title == "Metro 2033"))
            {
                var book2 = new Book
                {
                    IsAvailable = true,
                    Title = "Metro 2033",
                    WhoTookIt = null,
                    Year = 2033,
                    BookAuthors = null
                };
                _context.Books.Add(book2);
            }

            if (!_context.Books.Any(b => b.Title == "O psie, który jeździł kolejom"))
            {
                var book3 = new Book
                {
                    IsAvailable = true,
                    Title = "O psie, który jeździł kolejom",
                    WhoTookIt = null,
                    Year = 1998,
                    BookAuthors = null
                };
                _context.Books.Add(book3);
            }

            _context.SaveChanges();

        }
    }
}
