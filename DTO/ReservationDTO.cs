namespace bookstopAPI.DTOs
{
  public class ReservationDTO
  {
    public int Id { get; set; }
    public int BookId { get; set; }
    public required string UserId { get; set; }
    public int Days { get; set; }
    public bool IsAudiobook { get; set; }
    public bool isQuickPickUp { get; set; }
    public decimal TotalCost { get; set; }
  }
}