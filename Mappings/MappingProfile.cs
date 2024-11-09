using System.Runtime;
using AutoMapper;
using bookstopAPI.DTOs;
using bookstopAPI.Models;

public class MappingPRofile : ProfileOptimization{
  public MappingProfile()
  {
    CreateMap<Book,BookDTO>().ReverseMap();
  }
}