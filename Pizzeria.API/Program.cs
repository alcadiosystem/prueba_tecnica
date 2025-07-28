
using Microsoft.EntityFrameworkCore;
using Pizzeria.Application.Interfaces;
using Pizzeria.Application.Services;
using Pizzeria.Domain.Interfaces;
using Pizzeria.Infrastructure.Data;
using Pizzeria.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(
    it => it.UseNpgsql(builder.Configuration.GetConnectionString("LocalConnection"))
    );

//Registrar servicios
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IProductoService, ProductoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();


app.Run();

