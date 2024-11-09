using bookstopAPI.Data;
using bookstopAPI.Seed;
using DotNetEnv;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices();

var app = builder.Build();

app.ConfigureMiddleware();

SeedData.SeedDatabase(app);

app.Run();
