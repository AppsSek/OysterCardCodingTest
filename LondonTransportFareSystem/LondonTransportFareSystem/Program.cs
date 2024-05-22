using LondonTransportFareSystem.BLL;
using LondonTransportFareSystem.DAL;
using LondonTransportFareSystem.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

////EF CORE
builder.Services.AddScoped<ProcessTransaction, ProcessTransaction>();
builder.Services.AddDbContext<LondonTransportContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

builder.Services.AddScoped<Transaction, Transaction>();
builder.Services.AddScoped<CustomerTransactionDAL, CustomerTransactionDAL>();
builder.Services.AddScoped<SqlConnection, SqlConnection>();

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
