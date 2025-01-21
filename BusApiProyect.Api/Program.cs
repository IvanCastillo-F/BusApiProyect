using BusApiProyect.Data.Interfaces;
using BusApiProyect.Data.Models;
using BusApiProyect.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Add bd context to the proyect
builder.Services.AddTransient<DBContext>();

//Add user methods to the proyect
builder.Services.AddTransient<IUserRepository, UserRepository>();

//Add Bus Methods to the proyect
builder.Services.AddTransient<IBusRepository, BusRepository>();

//Add Route Methods to the proyect
builder.Services.AddTransient<IRouteRepository, RouteRepository>();

//Add Bus Schedule Methods to the proyect
builder.Services.AddTransient<IBusScheduleRepository, BusScheduleRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
