using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace libraryAPI.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public ICollection<BookAuthor> BookAuthors { get; set; }
        public bool IsAvailable { get; set; }
        public string WhoTookIt { get; set; }
        public Book()
        {
            BookAuthors = new Collection<BookAuthor>();
        }
    }
}
