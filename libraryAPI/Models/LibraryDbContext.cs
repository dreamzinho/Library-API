using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using libraryAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace libraryAPI.Models
{
    public class LibraryDbContext : IdentityDbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            :base(options)
        {
        }

        public DbSet<Book> Books { get; set; }

        public DbSet<libraryAPI.Entities.User> User { get; set; }
    }
}
