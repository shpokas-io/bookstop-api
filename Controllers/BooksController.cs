using bookstopAPI.Data;
using bookstopAPI.DTOs;
using bookstopAPI.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly LibraryContext _context;
private readonly IAdHocMapper _mapper;
    public BooksController(LibraryContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooks()
    {
        var books = await _context.Books.ToListAsync();
        var bookDTOs = _mapper.Map<IEnumerable<BookDTO>>(books);
        return Ok(bookDTOs);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookDTO>> GetBook(int id)
    {
      var book = await _context.Books.FindAsync(id);
      if(book == null)
      return NotFound();

      var bookDTO = _mapper.Map<bookDTO>>(book);
      return Ok(bookDTO);
    }

    [HttpPost]
    public async Task<ActionResult<BookDTO>> AddBook(BookDTO newBookDTO)
    {
      var newBook = _mapper.Map<Book>(newBookDTO);
      _context.Books.Add(newBook);
      await _context.SaveChangesAsync();

      var createBookDTO = _mapper.Map<newBookDTO>(newBook);
      return CreatedAtAction(nameof(GetBook), new {id = createdBookDTO.id}, createBookDTO);
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
