using bookstopAPI.Data;
using bookstopAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

//Enable CORS for syncing with the frontend application
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
    builder => 
    {
        builder.WithOrigins("https://bookspot-webapp.netlify.app")// FrontEnd port
        .AllowAnyHeader()//Allow any fetch header
        .AllowAnyMethod();//Allow any methods(GET,POST,DELETE etc.)
    });
});


//Configure the in-memory database EF CORE
builder.Services.AddDbContext<LibraryContext>(options => options.UseInMemoryDatabase("LibraryDB"));

var app = builder.Build();



//Seed data into the in-memory database
using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<LibraryContext>();
    SeedData(context);
}
// MEthod to seed initial book data into the database
void SeedData(LibraryContext context)
{
    //check if the db already contains any books to avoid seeding again
    if(!context.Books.Any())
    {
        context.Books.AddRange(new List<Book>
        {
            new Book { Id = 1, Name = "Harry Potter and the Sorcerer's Stone", Year = 1997, Type = "Book", PictureUrl= "https://m.media-amazon.com/images/I/91wKDODkgWL._SY466_.jpg" },
            new Book { Id = 2, Name = "Harry Potter and the Chamber of Secrets", Year = 1998, Type = "Book", PictureUrl= "https://m.media-amazon.com/images/I/918wxhKJaPL._SY466_.jpg" },
            new Book { Id = 3, Name = "The Lord of the Rings: The Fellowship of the Ring", Year = 1954, Type = "Book", PictureUrl= "https://m.media-amazon.com/images/I/41qCdBemr9L._SY445_SX342_.jpg" },
            new Book { Id = 4, Name = "The Lord of the Rings: The Two Towers", Year = 1954, Type = "Audiobook", PictureUrl= "https://m.media-amazon.com/images/I/41Lr-35uz8L._SY445_SX342_.jpg" }
        });
        context.SaveChanges(); //Saves changes to the database

    }
}



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); //Enable Swagger in development
    app.UseSwaggerUI();//Use Swagger UI
    
}

app.UseCors("AllowSpecificOrigin"); //Apply CORS policy

app.UseAuthorization();//Enable authorization
app.UseHttpsRedirection();//Redirect HTTP requests to more secure HTTPS

//Map API COntrollers
app.MapControllers();


app.Run();
