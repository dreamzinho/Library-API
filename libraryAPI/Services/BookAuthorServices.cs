using libraryAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace libraryAPI.Services
{
    public interface IBookAuthorService
    {
        bool AddAuthorToBook(Book book, Author author);
    }

    public class BookAuthorServices : IBookAuthorService
    {
        private readonly LibraryDbContext _context;

        public BookAuthorServices(LibraryDbContext context)
        {
            _context = context;
        }

        public bool AddAuthorToBook(Book book, Author author)
        {

            var bookAuthor = new BookAuthor
            {
                AuthorId = author.Id,
                BookId = book.Id,
                Author = author,
                Book = book
            };

            if (_context.BookAuthor.Any(ba => ba.AuthorId == bookAuthor.AuthorId && ba.BookId == bookAuthor.BookId)) return false;

            book.BookAuthors.Add(bookAuthor);
            author.BookAuthors.Add(bookAuthor);
            _context.BookAuthor.Add(bookAuthor);
            _context.SaveChanges();

            return true;
        }

    }
}
