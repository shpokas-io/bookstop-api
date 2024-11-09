using bookstopAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
  public static class ServiceExtensions
  {
    public static void ConfigureServices(this IServiceCollection services)
    {
      services.AddEndpointsApiExplorer();
      services.AddSwaggerGen();
      services.AddControllers();
      services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

      var allowedOrigin = Environment.GetEnvironmentVariable("ALLOWED_ORIGIN") ?? "http://localhost:3000";
      services.AddCors(oprions =>
      {
        options.AddPolicy("AllowSpecificOrigin", UriBuilder =>
        {
          UriBuilder.WithOrigins(allowedOrigin)
          .AllowAnyHeader()
          .AllowAnyHeader();
        });
      });
      services.AddDbContext<LibraryContext>(Options => Options.UseInMemoryDatabase("LibraryDB"));
    }
    public static void ConfigureMiddleware(this WebApplication app)
    {
      if(app.Environment.IsDevelopment())
      {
        app.UseSwagger();
        app.UseSwaggerUI();
      }
      app.UseCors("AllowSpecificOrigin");
            app.UseAuthorization();
            app.UseHttpsRedirection();
            app.MapControllers();
    }
  }
}