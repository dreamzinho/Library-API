using libraryAPI.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace libraryAPI.Services
{

    public interface IBookServices
    {
        int BorrowBook(User currentUser, int id);
    }
    public class BookServices : IBookServices
    {
        private readonly LibraryDbContext _context;

        public BookServices(LibraryDbContext context)
        {
            _context = context;
        }


        public int BorrowBook(User currentUser, int id)
        {

            return 1;

        }
        
    }
}
