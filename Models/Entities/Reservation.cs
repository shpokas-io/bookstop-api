
public class Reservation
{
  public int Id {get;set;}
  public int BookId {get;set;}
  public string UserId {get;set;}
  public bool IsAudiobook {get;set;}
  public int Days {get;set;}
  public bool IsQuickPickUp {get;set;}
  public decimal TotalCost {get;set;}
  public string BookName { get; set; }
  public string BookPictureUrl { get; set; }
}
