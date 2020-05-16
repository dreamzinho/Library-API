using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace libraryAPI.Entities
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public ICollection<BookAuthor> books { get; set; }
        public Author()
        {
            books = new Collection<BookAuthor>();
        }
    }
}
