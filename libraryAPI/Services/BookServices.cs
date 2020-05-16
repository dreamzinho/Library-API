using libraryAPI.DTOs;
using libraryAPI.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace libraryAPI.Services
{

    public interface IBookServices
    {
        int BorrowBook(User currentUser, int id);
        int PostBook(BookDTO book);
    }
    public class BookServices : IBookServices
    {
        private readonly LibraryDbContext _context;

        public BookServices(LibraryDbContext context)
        {
            _context = context;
        }

        public int PostBook(BookDTO book)
        {
            var newBook = new Book
            {
                Title = book.Title,
                Year = book.Year,
                WhoTookIt = book.WhoTookIt,
                IsAvailable = book.IsAvailable
            };
            foreach (var a in book.Authors)
            {
                var ba = new BookAuthor
                {
                    //Author = _context.Authors.Single(a => a.Id == a.Id),
                    AuthorId = a.Id,
                    BookId = newBook.Id
                };
                newBook.BookAuthors.Add(ba);
                _context.BookAuthor.Add(ba);
            }

            _context.Books.Add(newBook);
            _context.SaveChanges();

            return book.Id;
        }

        public int BorrowBook(User currentUser, int id)
        {

            return 1;

        }
        
    }
}
