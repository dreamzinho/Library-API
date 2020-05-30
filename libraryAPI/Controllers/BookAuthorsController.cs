using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using libraryAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using libraryAPI.Services;

namespace libraryAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookAuthorsController : ControllerBase
    {
        private readonly LibraryDbContext _context;
        private readonly IBookAuthorService _bookAuthorService;

        public BookAuthorsController(LibraryDbContext context, IBookAuthorService bookAuthorService)
        {
            _context = context;
            _bookAuthorService = bookAuthorService;
        }

        // GET: api/BookAuthors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookAuthor>>> GetBookAuthor()
        {
            return await _context.BookAuthor.ToListAsync();
        }

        // GET: api/BookAuthors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookAuthor>> GetBookAuthor(int bookId, int authorId)
        {
            var bookAuthor = await _context.BookAuthor.FindAsync(bookId, authorId);

            if (bookAuthor == null)
            {
                return NotFound();
            }

            return bookAuthor;
        }

        // PUT: api/BookAuthors/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        //[Authorize(Roles = "Admin")]
        //[HttpPut("{bookId}/{authorId}")]
        //public async Task<IActionResult> PutBookAuthor(int bookId, int authorId, BookAuthor bookAuthor)
        //{
        //    if (bookId != bookAuthor.BookId && authorId != bookAuthor.AuthorId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(bookAuthor).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!BookAuthorExists(bookId, authorId))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/BookAuthors
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult<bool> AddAuthorToBook([FromQuery(Name = "bookId")]int bookId, [FromQuery(Name = "authorId")] int authorId)
        {
            if (bookId <= 0 || authorId <= 0) return BadRequest(false);

            var book = _context.Books.Find(bookId);
            var author = _context.Authors.Find(authorId);

            if (book == null || author == null) return NotFound(false);

            var isAdded = _bookAuthorService.AddAuthorToBook(book, author);
            if(isAdded) return Ok(isAdded);
            else return BadRequest(new { message = "This author is already added to that book" });



        }

        // DELETE: api/BookAuthors/5
        //[Authorize(Roles = "Admin")]
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<BookAuthor>> DeleteBookAuthor(int id)
        //{
        //    var bookAuthor = await _context.BookAuthor.FindAsync(id);
        //    if (bookAuthor == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.BookAuthor.Remove(bookAuthor);
        //    await _context.SaveChangesAsync();

        //    return bookAuthor;
        //}

        //private bool BookAuthorExists(int bookId, int authorId)
        //{
        //    return _context.BookAuthor.Any(e => e.BookId == bookId && e.AuthorId == authorId);
        //}
    }
}
