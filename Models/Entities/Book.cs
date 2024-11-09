
namespace bookstopAPI.Models
{
public class Book
{
  public   int Id {get;set;}
  public required string Name {get;set;}
  public  required int Year {get;set;}
  public required string Type {get;set;}
  public required string PictureUrl {get;set;}
}
}
