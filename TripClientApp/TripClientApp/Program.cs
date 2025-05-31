using Microsoft.EntityFrameworkCore;
using TripClientApp.Models;
using TripClientApp.Services;

namespace TripClientApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddControllers();
        builder.Services.AddOpenApi();
        builder.Services.AddDbContext<TripsDbContext>(opt =>
        {
            var connectionString = builder.Configuration.GetConnectionString("Default");
            opt.UseSqlServer(connectionString);
        });
        builder.Services.AddScoped<IDbService, DbService>();
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}