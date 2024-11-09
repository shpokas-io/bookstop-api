using bookstopAPI.Data;
using bookstopAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly LibraryContext _context;

    public BooksController(LibraryContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
    {
        var books = await _context.Books.ToListAsync();
        return Ok(books);
    }

    [HttpPost]
    public async Task<ActionResult<Book>> AddBook(Book newBook)
    {
        if (newBook == null)
            return BadRequest("Book data is invalid.");

        _context.Books.Add(newBook);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBooks), new { id = newBook.Id }, newBook);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Book>>> SearchBooks(string? name, int? year, string? type)
    {
        var query = _context.Books.AsQueryable();

        if (!string.IsNullOrEmpty(name))
            query = query.Where(b => b.Name.Contains(name));

        if (year.HasValue)
            query = query.Where(b => b.Year == year.Value);

        if (!string.IsNullOrEmpty(type))
            query = query.Where(b => b.Type.Equals(type, StringComparison.OrdinalIgnoreCase));

        var result = await query.ToListAsync();

        return result.Any() ? Ok(result) : NotFound("No books match the search criteria");
    }
}
