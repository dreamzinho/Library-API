using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using libraryAPI.Entities;
using Microsoft.AspNetCore.Authorization;

namespace libraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookAuthorsController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BookAuthorsController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: api/BookAuthors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookAuthor>>> GetBookAuthor()
        {
            return await _context.BookAuthor.ToListAsync();
        }

        // GET: api/BookAuthors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookAuthor>> GetBookAuthor(int id)
        {
            var bookAuthor = await _context.BookAuthor.FindAsync(id);

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
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookAuthor(int id, BookAuthor bookAuthor)
        {
            if (id != bookAuthor.Id)
            {
                return BadRequest();
            }

            _context.Entry(bookAuthor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookAuthorExists(id))
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

        // POST: api/BookAuthors
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<BookAuthor>> PostBookAuthor(BookAuthor bookAuthor)
        {
            _context.BookAuthor.Add(bookAuthor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBookAuthor", new { id = bookAuthor.Id }, bookAuthor);
        }

        // DELETE: api/BookAuthors/5
        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<BookAuthor>> DeleteBookAuthor(int id)
        {
            var bookAuthor = await _context.BookAuthor.FindAsync(id);
            if (bookAuthor == null)
            {
                return NotFound();
            }

            _context.BookAuthor.Remove(bookAuthor);
            await _context.SaveChangesAsync();

            return bookAuthor;
        }

        private bool BookAuthorExists(int id)
        {
            return _context.BookAuthor.Any(e => e.Id == id);
        }
    }
}
