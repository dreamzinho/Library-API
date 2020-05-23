using libraryAPI.DTOs;
using libraryAPI.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using libraryAPI.Controllers;

namespace libraryAPI.Services
{

    public interface IBookServices
    {
        (Book,bool) BorrowBook(int bookId, string userId);
        (Book, bool) ReturnBook(int bookId);
        List<BookDTO> GetBorrowedBooks(string userId);
        int PostBook(BookDTO book);
        List<BookDTO> GetAll();
        BookDTO GetOne(int id);
        List<BookDTO> GetFilteredBooks( int[]? authorsIds, string? title, int? year);
        Book EditBook(int bookId, BookDTO bookDTO);
        Task<BookDTO> RemoveBook(int bookId);
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

        public List<BookDTO> GetAll()
        {
            var books = _context.Books.Select(b =>
            new BookDTO()
            {
                Id = b.Id,
                IsAvailable = b.IsAvailable,
                Title = b.Title,
                WhoTookIt = b.WhoTookIt,
                Year = b.Year,
            }).ToList();

            int idx = 0;
            foreach (Book b in _context.Books.Include(ba => ba.BookAuthors).ToList())
            {
                List<AuthorDTO> Authors = new List<AuthorDTO>();

                    Authors = _context.BookAuthor.Include(a => a.Author).Where(ba => ba.BookId == b.Id).Select(a =>
                    new AuthorDTO
                    {
                        Id = a.AuthorId,
                        Name = a.Author.Name,
                        Surname = a.Author.Surname
                    }).ToList();


                books[idx++].Authors = Authors;
            }

            return books;
        }

        public BookDTO GetOne(int id)
        {
            var book = _context.Books.Where(b => b.Id == id).Select(b =>
            new BookDTO
            {
                Id = b.Id,
                IsAvailable = b.IsAvailable,
                Title = b.Title,
                WhoTookIt = b.WhoTookIt,
                Year = b.Year,
                Authors = _context.BookAuthor.Include(a => a.Author).Where(ba => ba.BookId == b.Id).Select(a =>
                    new AuthorDTO
                    {
                        Id = a.AuthorId,
                        Name = a.Author.Name,
                        Surname = a.Author.Surname
                    }).ToList()
            }).SingleOrDefault();



            return book;
        }

        public List<BookDTO> GetFilteredBooks(int[]? authorsIds, string? title, int? year)
        {
            var filteredBooks = _context.Books.Include(b => b.BookAuthors).ToList();
            List<Book> result = filteredBooks.ToList();

            if (year != 0) 
            {
                filteredBooks = filteredBooks.Where(fb => fb.Year == year).ToList();
                result = filteredBooks;
            }
            if (!String.IsNullOrEmpty(title)) 
            {
                filteredBooks = filteredBooks.Where(fb => fb.Title == title).ToList();
                result = filteredBooks;
            };
            if(authorsIds.Length > 0)
            {
                //Bardzo nieoptymalne rozwiązanie

                //int counter;
                //result = new List<Book>();
                //foreach (Book book in filteredBooks)
                //{
                //    counter = 0;

                //    for (int i = 0; i < authorsIds.Length; i++)
                //    {
                //        for (int j = 0; j < book.BookAuthors.Count(); j++)
                //        {
                //            if (authorsIds[i] == book.BookAuthors.ToList()[j].AuthorId) counter++;
                //        }
                //    }
                //    if(counter == authorsIds.Length && counter != 0)
                //    {
                //        result.Add(book);
                //    }
                //}

                //Chyba lepsze rozwiązania

                //gdy lista identyfikatorów autorów danej książki jest równa liście podanej jako parametr (są sobie równe)
                //filteredBooks = filteredBooks.Where(fb => Enumerable.SequenceEqual(fb.BookAuthors.Select(a => a.AuthorId).ToArray(), authorsIds.)).ToList();

                //gdy lista podana jako parametr zawiera się w liście identyfikatorów autorów danej książki (jest podzbiorem)
                filteredBooks = filteredBooks.Where(fb => authorsIds.All(id => fb.BookAuthors.Select(ba => ba.AuthorId).ToArray().Contains(id))).ToList();

                result = filteredBooks;
            }

            var books = result.Select(r =>
            new BookDTO
            {
                Id = r.Id,
                IsAvailable = r.IsAvailable,
                Title = r.Title,
                WhoTookIt = r.WhoTookIt,
                Year = r.Year,
                Authors = _context.BookAuthor.Include(a => a.Author).Where(ba => ba.BookId == r.Id).Select(a =>
                        new AuthorDTO
                        {
                            Id = a.AuthorId,
                            Name = a.Author.Name,
                            Surname = a.Author.Surname
                        }).ToList()
            }).ToList();


            return books;
        }

        public (Book,bool) BorrowBook(int bookId, string userId)
        {

            var book = _context.Books.Where(b => b.Id == bookId).FirstOrDefault();

            if (book == null) return (null,false);

            if (book.IsAvailable == false) return (book,false);

            book.IsAvailable = false;
            book.WhoTookIt = userId;

            _context.SaveChanges();

            return (book,true);

        }

        public (Book, bool) ReturnBook(int bookId)
        {
            var book = _context.Books.Where(b => b.Id == bookId).FirstOrDefault();

            if (book == null) return (null, false);

            if (book.IsAvailable == true) return (book, false);

            book.IsAvailable = true;
            book.WhoTookIt = "";

            _context.SaveChanges();

            return (book, true);
        }

        public List<BookDTO> GetBorrowedBooks(string userId)
        {
            var borrowedBooks = _context.Books.Where(b => b.WhoTookIt.Equals(userId)).Select(b =>
            new BookDTO
            {
                Id = b.Id,
                WhoTookIt = b.WhoTookIt,
                IsAvailable = b.IsAvailable,
                Title = b.Title,
                Year = b.Year,
                Authors = _context.BookAuthor.Include(a => a.Author).Where(ba => ba.BookId == b.Id).Select(a =>
                        new AuthorDTO
                        {
                            Id = a.AuthorId,
                            Name = a.Author.Name,
                            Surname = a.Author.Surname
                        }).ToList()
            }).ToList();

            return borrowedBooks;
        }

        public Book EditBook(int bookId, BookDTO bookDTO)
        {
            var book = _context.Books.Where(b => b.Id == bookId).FirstOrDefault();

            book.IsAvailable = bookDTO.IsAvailable;
            book.Title = bookDTO.Title;
            book.WhoTookIt = bookDTO.WhoTookIt;
            book.Year = bookDTO.Year;

            while(_context.BookAuthor.Any(ba => ba.BookId == book.Id))
            {
                var bookAuthor = _context.BookAuthor.Where(ba => ba.BookId == book.Id).FirstOrDefault();
                _context.BookAuthor.Remove(bookAuthor);
                _context.SaveChanges();
            }
            book.BookAuthors = new List<BookAuthor>();
            foreach (var author in bookDTO.Authors)
            {
                var ba = new BookAuthor
                {
                    BookId = book.Id,
                    AuthorId = author.Id
                };
                book.BookAuthors.Add(ba);
                _context.BookAuthor.Add(ba);
            }

            _context.SaveChanges();
            return book;

        }

        public async Task<BookDTO> RemoveBook(int bookId)
        {
            var book = await _context.Books.Include(ba => ba.BookAuthors).Where(b => b.Id == bookId).FirstOrDefaultAsync();

            if (book == null) return null;

            var bookDTO = new BookDTO
            {
                Id = book.Id,
                IsAvailable = book.IsAvailable,
                Title = book.Title,
                WhoTookIt = book.WhoTookIt,
                Year = book.Year,
                Authors = _context.BookAuthor.Include(a => a.Author).Where(ba => ba.BookId == book.Id).Select(a =>
                        new AuthorDTO
                        {
                            Id = a.AuthorId,
                            Name = a.Author.Name,
                            Surname = a.Author.Surname
                        }).ToList()
            };

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();


            return bookDTO;
        }
    }
}
