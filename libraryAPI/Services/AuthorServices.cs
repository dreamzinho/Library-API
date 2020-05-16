using libraryAPI.DTOs;
using libraryAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace libraryAPI.Services
{
    public interface IAuthorServices
    {
        Task<int> PostAuthor(AuthorDTO author);
    }


    public class AuthorServices : IAuthorServices
    {
        private readonly LibraryDbContext _context;
        public AuthorServices(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<int> PostAuthor(AuthorDTO author)
        {
            var author1 = new Author
            {
                Name = author.Name,
                Surname = author.Surname,
            };

            await _context.AddAsync(author1);
            _context.SaveChanges();

            return author1.Id;
        }
    }
}
