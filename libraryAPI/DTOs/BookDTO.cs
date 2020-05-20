using libraryAPI.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace libraryAPI.DTOs
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public ICollection<AuthorDTO> Authors { get; set; }
        public bool IsAvailable { get; set; }
        public string WhoTookIt { get; set; }
        public BookDTO()
        {
            Authors = new Collection<AuthorDTO>();
        }
    }
}
