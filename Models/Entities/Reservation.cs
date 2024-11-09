//Public class for reservation 
public class Reservation
{
  public int Id {get;set;}
  public int BookId {get;set;}
  public string UserId {get;set;}
  public bool IsAudiobook {get;set;}//Audio or Book
  public int Days {get;set;} //number of days
  public bool IsQuickPickUp {get;set;}
  public decimal TotalCost {get;set;}
  public string BookName { get; set; }
  public string BookPictureUrl { get; set; }
}
