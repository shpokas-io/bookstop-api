using bookstopAPI.Data;
using bookstopAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookstopAPI.Seed
{
    public static class SeedData
    {
        public static async Task SeedDatabaseAsync(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<LibraryContext>();

            if (!context.Books.Any())
            {
                var books = GetSeedBooks();
                await context.Books.AddRangeAsync(books);
                await context.SaveChangesAsync();
            }
        }

        private static List<Book> GetSeedBooks()
        {
            return new List<Book>
            {
                new Book { Name = "Harry Potter and the Sorcerer's Stone", Year = 1997, Type = "Book", PictureUrl = "https://m.media-amazon.com/images/I/91wKDODkgWL._SY466_.jpg" },
                new Book { Name = "Harry Potter and the Chamber of Secrets", Year = 1998, Type = "Book", PictureUrl = "https://m.media-amazon.com/images/I/918wxhKJaPL._SY466_.jpg" },
                new Book { Name = "The Lord of the Rings: The Fellowship of the Ring", Year = 1954, Type = "Book", PictureUrl = "https://m.media-amazon.com/images/I/41qCdBemr9L._SY445_SX342_.jpg" },
                new Book { Name = "The Lord of the Rings: The Two Towers", Year = 1954, Type = "Audiobook", PictureUrl = "https://m.media-amazon.com/images/I/41Lr-35uz8L._SY445_SX342_.jpg" }
            };
        }
    }
}
