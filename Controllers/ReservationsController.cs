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

  //Post api/Reservations
  [HttpPost]
  public async Task<ActionResult<Reservation>> CreateReservation(Reservation reservation)
  {
    //Validate reservation details
if(reservation ==null || reservation.Days <= 0 || string.IsNullOrWhiteSpace(reservation.UserId))
{
  return BadRequest("Invalid reservation details.");
}
    //Find the associated book
    var book = await _context.Books.FindAsync(reservation.BookId);
    if(book == null)
    {
      return NotFound("Book not found.");
    }

    //Calculate total cost based on days, type, quick pickup, etc.
    decimal dailyRate = reservation.IsAudiobook ? 3m : 2m;
    decimal totalCost = dailyRate * reservation.Days;

    //Apply discounts based on the number of days reserved
    if (reservation.Days > 3)
    {
      totalCost -= totalCost * 0.1m; //10% discount for > 3 days
    }
    if(reservation.Days > 10)
    {
      totalCost -= totalCost * 0.2m; //20% deiscount for > 10 days
    }

    //Add fixed service fee and quick pickup fee if applicable
    totalCost += 3m; //This is service fee default
    if(reservation.IsQuickPickUp)
    {
      totalCost += 5m; //Quick pickup fee
    }

    //Set the total cost in the reservation object
    reservation.TotalCost = totalCost;

    //Add the reservation to the database
    _context.Reservations.Add(reservation);
    await _context.SaveChangesAsync();

    //Return the created reservation
    return CreatedAtAction("GetReservation", new { id = reservation.Id}, reservation);
 
  }

  //GET: api/Reservations
  [HttpGet]
  public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
  {
    //Return list of reservations
    return await _context.Reservations.ToListAsync();
  }

  //GET: api/Reservations/5
  [HttpGet("{id}")]
  public async Task<ActionResult<Reservation>> GetReservation(int id)
  {
    //Find the reservation by ID
    var reservation = await _context.Reservations.FindAsync(id);


    //CHeck if the reservation by ID
    if(reservation == null)
    {
      return NotFound();

    }

    //Return the found reservation
    return reservation;
  }

//DELETE: api/reservations/{id}
  [HttpDelete("{id}")]
  public async Task<ActionResult> DeleteReservation(int id)
  {
    //Find the reservation by ID
    var reservation = await _context.Reservations.FindAsync(id);

    if(reservation == null)
    {
      return NotFound();
    }
    //Remove the reservation from the database
    _context.Reservations.Remove(reservation);
    await _context.SaveChangesAsync();

    return NoContent();
  }
}