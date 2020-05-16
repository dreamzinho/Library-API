using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace libraryAPI.Entities
{
    public class BookAuthor
    {
        public int BookId { get; set; }
        public int AuthorId { get; set; }

        public Author Author { get; set; }
        public Book Book { get; set; }
    }
}
