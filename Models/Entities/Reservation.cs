
public class Reservation
{
  public int Id {get;set;}
  public int BookId {get;set;}
  public required string UserId {get;set;}
  public bool IsAudiobook {get;set;}
  public int Days {get;set;}
  public bool IsQuickPickUp {get;set;}
  public decimal TotalCost {get;set;}
  public required string BookName { get; set; }
  public  required string BookPictureUrl { get; set; }
}
