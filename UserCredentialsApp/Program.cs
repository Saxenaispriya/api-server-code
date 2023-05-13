using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UserCredentialsApp.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnstring")));


// Add services to the container.

builder.Services.AddControllers();

//builder.Services.AddDbContext<DatabaseContext>(option =>
//option.Options.UseInMemoryDatabase("DefaultConnstring"));



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();