namespace bookstopAPI.DTOs
{
  public class BookDTO
  {
    public int Id { get; set;}
    public required string Name {get; set;}
    public int Year {get; set;}
    public required string Type {get; set;}
    public required string PictureUrl {get; set;}
    
  }
}