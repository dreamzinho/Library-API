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
        public DbSet<BookAuthor> BookAuthor { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<BookAuthor>()
                 .HasKey(ba => new { ba.BookId, ba.AuthorId });
            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.Book)
                .WithMany(ba => ba.BookAuthors)
                .HasForeignKey(ba => ba.BookId);
            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.Author)
                .WithMany(ba => ba.BookAuthors)
                .HasForeignKey(ba => ba.AuthorId);


            modelBuilder.Entity<BookAuthor>()
                .HasOne<Book>(b => b.Book)
                .WithMany(c => c.BookAuthors)
                .Metadata.DeleteBehavior = DeleteBehavior.Cascade;

            modelBuilder.Entity<BookAuthor>()
               .HasOne<Author>(a => a.Author)
               .WithMany(c => c.BookAuthors)
               .Metadata.DeleteBehavior = DeleteBehavior.Cascade;



            base.OnModelCreating(modelBuilder);
        }


    }
}
