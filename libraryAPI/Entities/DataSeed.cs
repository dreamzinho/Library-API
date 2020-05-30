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
        public static void SeedBooks(LibraryDbContext _context)
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

        public static void SeedAuthors(LibraryDbContext _context)
        {
            if (!_context.Authors.Any(a => a.Name == "Adam" && a.Surname == "Mickiewicz"))
            {
                var author1 = new Author
                {
                    Name = "Adam",
                    Surname = "Mickiewicz",
                    BookAuthors = null
                };
                _context.Authors.Add(author1);
            }

            if (!_context.Authors.Any(a => a.Name == "Bolesław" && a.Surname == "Prus"))
            {
                var author2 = new Author
                {
                    Name = "Bolesław",
                    Surname = "Prus",
                    BookAuthors = null
                };
                _context.Authors.Add(author2);
            }

            if (!_context.Authors.Any(a => a.Name == "Juliusz" && a.Surname == "Słowacki"))
            {
                var author3 = new Author
                {
                    Name = "Juliusz",
                    Surname = "Słowacki",
                    BookAuthors = null
                };
                _context.Authors.Add(author3);
            }

            _context.SaveChanges();

        }
        public static void SeedBookAuthor(LibraryDbContext _context)
        {

            var author1 = _context.Authors.Where(a => a.Name == "Adam" && a.Surname == "Mickiewicz").FirstOrDefault();
            var book1 = _context.Books.Where(a => a.Title == "Metro 2033").FirstOrDefault();
            var author2 = _context.Authors.Where(a => a.Name == "Bolesław" && a.Surname == "Prus").FirstOrDefault();
            var book2 = _context.Books.Where(a => a.Title == "Dzieci Z Bulerbyn").FirstOrDefault();
            var author3 = _context.Authors.Where(a => a.Name == "Juliusz" && a.Surname == "Słowacki").FirstOrDefault();
            var book3 = _context.Books.Where(a => a.Title == "O psie, który jeździł kolejom").FirstOrDefault();

            if (!_context.BookAuthor.Any(ba => ba.BookId == book1.Id && ba.AuthorId == author1.Id))
            {
                var ba1 = new BookAuthor
                {
                    AuthorId = author1.Id,
                    BookId = book1.Id,
                    Author = author1,
                    Book = book1
                };
                
                _context.BookAuthor.Add(ba1);
                author1.BookAuthors.Add(ba1);
                book1.BookAuthors.Add(ba1);
            }

            if (!_context.BookAuthor.Any(ba => ba.BookId == book2.Id && ba.AuthorId == author1.Id) &&
                !_context.BookAuthor.Any(ba => ba.BookId == book2.Id && ba.AuthorId == author2.Id))
            {
                var ba1 = new BookAuthor
                {
                    AuthorId = author1.Id,
                    BookId = book2.Id,
                    Author = author1,
                    Book = book2
                };

                var ba2 = new BookAuthor
                {
                    AuthorId = author2.Id,
                    BookId = book2.Id,
                    Author = author2,
                    Book = book2
                };

                _context.BookAuthor.Add(ba1);
                author1.BookAuthors.Add(ba1);
                book2.BookAuthors.Add(ba1);

                _context.BookAuthor.Add(ba2);
                author2.BookAuthors.Add(ba2);
            }

            if (!_context.BookAuthor.Any(ba => ba.BookId == book3.Id && ba.AuthorId == author1.Id) &&
                !_context.BookAuthor.Any(ba => ba.BookId == book3.Id && ba.AuthorId == author2.Id) &&
                !_context.BookAuthor.Any(ba => ba.BookId == book3.Id && ba.AuthorId == author3.Id))
            {
                var ba1 = new BookAuthor
                {
                    AuthorId = author1.Id,
                    BookId = book3.Id,
                    Author = author1,
                    Book = book3
                };

                var ba2 = new BookAuthor
                {
                    AuthorId = author2.Id,
                    BookId = book3.Id,
                    Author = author2,
                    Book = book3
                };

                var ba3 = new BookAuthor
                {
                    AuthorId = author3.Id,
                    BookId = book3.Id,
                    Author = author3,
                    Book = book3
                };

                _context.BookAuthor.Add(ba1);
                author1.BookAuthors.Add(ba1);

                _context.BookAuthor.Add(ba2);
                author2.BookAuthors.Add(ba2);

                _context.BookAuthor.Add(ba3);
                author3.BookAuthors.Add(ba3);
                book3.BookAuthors.Add(ba3);
            }

            _context.SaveChanges();

        }
    }
}
