using bookstopAPI.Data;
using bookstopAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly LibraryContext _context;

    public ReservationsController(LibraryContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<Reservation>> CreateReservation(Reservation reservation)
    {
        if (reservation == null || reservation.Days <= 0 || string.IsNullOrWhiteSpace(reservation.UserId))
            return BadRequest("Invalid reservation details.");

        var book = await _context.Books.FindAsync(reservation.BookId);
        if (book == null)
            return NotFound("Book not found.");

        decimal dailyRate = reservation.IsAudiobook ? 3m : 2m;
        reservation.TotalCost = CalculateTotalCost(dailyRate, reservation.Days, reservation.IsQuickPickUp);

        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id }, reservation);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
    {
        return Ok(await _context.Reservations.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Reservation>> GetReservation(int id)
    {
        var reservation = await _context.Reservations.FindAsync(id);
        return reservation == null ? NotFound() : Ok(reservation);
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
