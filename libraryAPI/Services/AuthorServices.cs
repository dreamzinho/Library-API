using libraryAPI.DTOs;
using libraryAPI.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace libraryAPI.Services
{
    public interface IAuthorServices
    {
        int PostAuthor(AuthorDTO author);
        List<AuthorDTO> GetAll();
        Task<AuthorDTO> GetOne(int authorId);
        Task<Author> EditAuthor(int authorId, AuthorDTO authorDTO);
        Task<AuthorDTO> RemoveAuthor(int authorId);
    }


    public class AuthorServices : IAuthorServices
    {
        private readonly LibraryDbContext _context;
        public AuthorServices(LibraryDbContext context)
        {
            _context = context;
        }

        public List<AuthorDTO> GetAll()
        {
            var authors =  _context.Authors.Select(a =>
            new AuthorDTO
            {
                Id = a.Id,
                Name = a.Name,
                Surname = a.Surname
            }).ToList();

            return authors;
        }

        public async Task<AuthorDTO> GetOne(int authorId)
        {
            var author = await _context.Authors.FindAsync(authorId);

            if (author == null) return null;

            var authorDTO = new AuthorDTO
            {
                Id = author.Id,
                Name = author.Name,
                Surname = author.Surname
            };

            return authorDTO;
        }

        public int PostAuthor(AuthorDTO author)
        {
            var author1 = new Author
            {
                Id = author.Id,
                Name = author.Name,
                Surname = author.Surname,
                BookAuthors = null
            };

            _context.Authors.Add(author1);
            _context.SaveChanges();

            return author1.Id;
        }

        public async Task<Author> EditAuthor(int authorId, AuthorDTO authorDTO)
        {
            var author = await _context.Authors.FindAsync(authorId);

            if (author == null) return null;

            author.Name = authorDTO.Name;
            author.Surname = authorDTO.Surname;

            await _context.SaveChangesAsync();

            return author;
        }

        public async Task<AuthorDTO> RemoveAuthor(int authorId)
        {
            var author = await _context.Authors.FindAsync(authorId);

            if (author == null) return null;

            var authorDTO = new AuthorDTO
            {
                Id = author.Id,
                Name = author.Name,
                Surname = author.Surname
            };

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return authorDTO;
        }
    }
}
