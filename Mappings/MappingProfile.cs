using System.Runtime;
using AutoMapper;
using bookstopAPI.DTOs;
using bookstopAPI.Models;

public class MappingProfile : Profile{
  public MappingProfile()
  {
    CreateMap<Book,BookDTO>().ReverseMap();
  }
}