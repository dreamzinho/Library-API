using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace libraryAPI.Entities
{
    public class BookAuthor
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int AuthorId { get; set; }
    }
}
