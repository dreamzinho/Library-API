using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using libraryAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using static libraryAPI.Services.BookServices;
using libraryAPI.Services;
using libraryAPI.DTOs;

namespace libraryAPI.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryDbContext _context;
        private readonly IBookServices _bookServices;

        public BooksController(LibraryDbContext context, IBookServices bookServices)
        {
            _context = context;
            _bookServices = bookServices;
        }

        // GET: api/Books
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<IEnumerable<BookDTO>> GetBooks()
        {
            var books = _bookServices.GetAll();
            if(books == null)
            {
                return NotFound();
            }

            return Ok(books);
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public ActionResult<BookDTO> GetBook(int id)
        {
            var book = _bookServices.GetOne(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }
        [HttpGet]
        [Route("filters")]
        public ActionResult<IEnumerable<BookDTO>> GetFilteredBooks([FromQuery(Name = "authorsIds")] int[] authorsIds,
                                                                   [FromQuery(Name = "title")] string title,
                                                                   [FromQuery(Name = "year")] int year)
        {
            var books = _bookServices.GetFilteredBooks(authorsIds, title, year);

            return Ok(books);
        }

        [HttpGet]
        [Route("borrow")]
        public ActionResult<bool> BorrowBook([FromQuery] int bookId,[FromQuery] string userId)
        {
            if (bookId <= 0 || String.IsNullOrEmpty(userId)) return BadRequest(false);

            var (borrowedBook,isBorrowed) = _bookServices.BorrowBook(bookId, userId);

            if (borrowedBook == null) return NotFound(isBorrowed);

            return Ok(isBorrowed);
        }

        [HttpGet]
        [Route("return")]
        public ActionResult<bool> ReturnBook([FromQuery] int bookId)
        {
            if (bookId <= 0) return BadRequest(false);

            var (returnedBook, isReturned) = _bookServices.ReturnBook(bookId);

            if (returnedBook == null) return NotFound(isReturned);

            return Ok(true);
        }


        // PUT: api/Books/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, BookDTO book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            var b = _bookServices.EditBook(id, book);

            _context.Entry(b).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public ActionResult<Book> PostBook(BookDTO book)
        {
            if (book == null) return BadRequest();
            if (_context.Books.Any(b => b.Title == book.Title)) return BadRequest(new { message = "Book already exists" });
            var Id = _bookServices.PostBook(book);
            return CreatedAtAction(nameof(GetBook), new { id = Id }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BookDTO>> DeleteBook(int id)
        {
            var book = await _bookServices.RemoveBook(id);

            if (book == null) return NotFound();

            return book;
        }

        [HttpGet]
        [Route("borrowedBooks")]
        public ActionResult<IEnumerable<BookDTO>> GetBorrowedBooks([FromQuery] string userId)
        {
            if (String.IsNullOrEmpty(userId)) return BadRequest();

            var books = _bookServices.GetBorrowedBooks(userId);

            if (books == null) return NotFound();

            return Ok(books);
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }

    }
}
