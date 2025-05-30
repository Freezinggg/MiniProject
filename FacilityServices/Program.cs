
using FacilityServices.Interface;
using FacilityServices.Service;
using Microsoft.EntityFrameworkCore;

namespace FacilityServices
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<FacilityDbContext>(options =>
                    options.UseNpgsql(builder.Configuration.GetConnectionString("DbConn")));

            builder.Services.AddScoped<IFacilityService, FacilityService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
