using bookstopAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace bookstopAPI.Data
{
  public class LibraryContext : DbContext
  {
    public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

    public DbSet<Book> Books { get;set; }
    public DbSet<Reservation> Reservations { get; set;}

    protected override void OnModelCreating (ModelBuilder modelBuilder)
    {

      modelBuilder.Entity<Book>().HasData(
        new Book { Id = 1, Name = "Harry Potter and the Sorcerer's Stone", Year = 1997, Type = "Book", PictureUrl= " https://covers.openlibrary.org/b/id/7984916-L.jpg "},
        new Book { Id = 2, Name = "Harry Potter and the Chamber of Secrets", Year = 1998, Type = "Book", PictureUrl= " https://covers.openlibrary.org/b/id/7984919-L.jpg "},
        new Book { Id = 3, Name = "The Lord of the Rings: The Fellowship of the Ring", Year = 1954, Type = "Book", PictureUrl= " https://covers.openlibrary.org/b/id/6202464-L.jpg "},
        new Book { Id = 4, Name = "The Lord of the Rings: The Two Towers", Year = 1954, Type = "Audiobook", PictureUrl= " https://covers.openlibrary.org/b/id/6202465-L.jpg "}
      );
    }
  }
}