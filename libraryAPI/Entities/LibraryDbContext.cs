using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using libraryAPI.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace libraryAPI.Entities
{
    public class LibraryDbContext : IdentityDbContext<IdentityUser>
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            :base(options)
        {
        }

        public DbSet<Book> Books { get; set; }

        public DbSet<User> User { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<libraryAPI.Entities.BookAuthor> BookAuthor { get; set; }

    }
}
