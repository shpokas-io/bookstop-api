using AutoMapper;
using bookstopAPI.Data;
using bookstopAPI.DTOs;
using bookstopAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly LibraryContext _context;
    private readonly IMapper _mapper;

    public ReservationsController(LibraryContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<ReservationDTO>> CreateReservation(ReservationDTO reservationDto)
    {
        try 
        {
            if(reservationDto == null || reservationDto.Days <= 0 || string.IsNullOrWhiteSpace(reservationDto.UserId))
            return BadRequest(new { message = "Invalid reservation details."});

            var book = await _context.Books.FindAsync(reservationDto.BookId);
            if(book == null)
            return NotFound(new{message = "Book not found."});

            var reservation = _mapper.Map<Reservation>(reservationDto);
            reservation.BookName = book.Name;
            reservation.BookPictureUrl = book.PictureUrl;
            decimal dailyRate = reservation.IsAudiobook ? 3m : 2m;
            reservation.TotalCost = CalculateTotalCost(dailyRate, reservation.Days, reservation.IsQuickPickUp);

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            var createdReservationDto = _mapper.Map<ReservationDTO>(reservation);
            return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id}, createdReservationDto);

        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error creating reservation: {ex.Message}");
            return StatusCode(500, new {message = "An error occurred while processing your request."});
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReservationDTO>>> GetReservations()
    {
        var reservations = await _context.Reservations.ToListAsync();
        var reservationDtos = _mapper.Map<IEnumerable<ReservationDTO>>(reservations);
        return Ok(reservationDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReservationDTO>> GetReservation(int id)
    {
        var reservation = await _context.Reservations.FindAsync(id);
        if (reservation == null)
            return NotFound();

        var reservationDto = _mapper.Map<ReservationDTO>(reservation);
        return Ok(reservationDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReservation(int id)
    {
        var reservation = await _context.Reservations.FindAsync(id);
        if (reservation == null)
            return NotFound();

        _context.Reservations.Remove(reservation);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private decimal CalculateTotalCost(decimal dailyRate, int days, bool isQuickPickUp)
    {
        decimal totalCost = dailyRate * days;

        if (days > 10)
            totalCost *= 0.8m;
        else if (days > 3)
            totalCost *= 0.9m;

        totalCost += 3m;
        if (isQuickPickUp)
            totalCost += 5m;

        return totalCost;
    }
}
