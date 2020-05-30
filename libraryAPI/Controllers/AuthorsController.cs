using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using libraryAPI.Entities;
using libraryAPI.DTOs;
using libraryAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace libraryAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly LibraryDbContext _context;
        private readonly IAuthorServices _authorServices;

        public AuthorsController(LibraryDbContext context, IAuthorServices authorServices)
        {
            _context = context;
            _authorServices = authorServices;
        }

        // GET: api/Authors
        [HttpGet]
        public ActionResult<IEnumerable<AuthorDTO>> GetAuthors()
        {
            var authors = _authorServices.GetAll();

            if (authors == null) return NotFound();

            return Ok(authors);
        }

        // GET: api/Authors/5
        [HttpGet("{authorId}")]
        public async Task<ActionResult<AuthorDTO>> GetAuthor(int authorId)
        {
            if (authorId <= 0) return BadRequest();

            var author = await _authorServices.GetOne(authorId);

            if (author == null)
            {
                return NotFound();
            }

            return Ok(author);
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{authorId}")]
        public async Task<IActionResult> PutAuthor(int authorId, AuthorDTO authorDTO)
        {
            if (authorId != authorDTO.Id || authorDTO == null) return BadRequest();

            var author = await _authorServices.EditAuthor(authorId, authorDTO);

            if (author == null) return NotFound();

            return NoContent();
        }

        // POST: api/Authors
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public ActionResult<Author> PostAuthor(AuthorDTO author)
        {
            if (author == null) return BadRequest();
            if (_context.Authors.Any(a => a.Name == author.Name && a.Surname == author.Surname)) return BadRequest(new { message = "Author already exists" });
            var Id = _authorServices.PostAuthor(author);

            return CreatedAtAction(nameof(GetAuthor), new { authorId = Id }, author);
        }

        // DELETE: api/Authors/5
        [HttpDelete("{authorId}")]
        public async Task<ActionResult<AuthorDTO>> DeleteAuthor(int authorId)
        {
            if (authorId <= 0) return BadRequest();

            var author = await _authorServices.RemoveAuthor(authorId);

            if (author == null) return NotFound();

            return Ok(author);
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.Id == id);
        }
    }
}
