using bookstopAPI.Data;
using bookstopAPI.Models;
using DotNetEnv;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices();

var app = builder.Build();

app.ConfigureMiddleware();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<LibraryContext>();
    if (!context.Books.Any())
    {
        context.Books.AddRange(new List<Book>
        {
            new Book { Name = "Harry Potter and the Sorcerer's Stone", Year = 1997, Type = "Book", PictureUrl = "https://covers.openlibrary.org/b/id/7984916-L.jpg" },
            new Book { Name = "Harry Potter and the Chamber of Secrets", Year = 1998, Type = "Book", PictureUrl = "https://covers.openlibrary.org/b/id/7984919-L.jpg" },
            new Book { Name = "The Lord of the Rings: The Fellowship of the Ring", Year = 1954, Type = "Book", PictureUrl = "https://covers.openlibrary.org/b/id/6202464-L.jpg" },
            new Book { Name = "The Lord of the Rings: The Two Towers", Year = 1954, Type = "Audiobook", PictureUrl = "https://covers.openlibrary.org/b/id/6202465-L.jpg" }
        });
        context.SaveChanges();
    }
}

app.Run();
