using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class AuthorService : IAuthorService
    {
        private readonly ApplicationDbContext _context;
        private readonly int _publishedDateYear = 2015;
        public AuthorService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Author> GetAuthor()
        {
            Author author = (await _context.Books.Include(book => book.Author)
            .OrderByDescending(book => book.Title.Length)
            .ThenBy(book => book.AuthorId)
            .FirstOrDefaultAsync())?.Author;

            return new Author // если так не сделать будет зацикливоние json
            {
                Id = author.Id,
                Surname = author.Surname,
                Name = author.Name
            };
        }



        public async Task<List<Author>> GetAuthors()
        {
            var authors = await _context.Books.Include(author => author.Author)
                                .Where(book => book.PublishDate.Year > _publishedDateYear)
                                .GroupBy(book => new { book.AuthorId, book.Author.Books.Count })
                                .Where(groups => groups.Key.Count % 2 == 0)
                                .Select(groups => groups.First().Author)
                                .ToListAsync();
            List<Author> authorsToDisplay = new(); // если так не сделать будет зацикливоние json
            authors.ForEach(author => authorsToDisplay.Add(new Author { Id = author.Id, Name = author.Name, Surname = author.Surname }));
            return authors;
        }
    }
}