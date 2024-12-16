using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;
        private readonly DateTime _sabatonCarolusRexAlbumPublishedDate = new DateTime(2012,5,22);

        public BookService(ApplicationDbContext context)
        {
            _context = context;   
        }
        public async Task<Book> GetBook() =>       
             await _context.Books.OrderByDescending(x => x.QuantityPublished * x.Price).FirstOrDefaultAsync();


        public async Task<List<Book>> GetBooks() =>
            await _context.Books.Where(book => book.PublishDate >= _sabatonCarolusRexAlbumPublishedDate && book.Title.Contains("Red")).ToListAsync();
    }
}
