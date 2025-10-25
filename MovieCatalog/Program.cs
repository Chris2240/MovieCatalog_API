
using Microsoft.EntityFrameworkCore;
using MovieCatalog.Data;
using MovieCatalog.Models;
using System.Text.Json;

namespace MovieCatalog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Register EF Core with InMemory database
            builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("MovieCatalogDb"));  // Use InMemory "MovieCatalogDb" while app is running

            // CORS: allow requests from your Blazor client origin to access this API in frontend calls
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowBlazorClient", policy =>
                {
                    policy.WithOrigins("http://localhost:5269")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                    // .AllowCredentials(); // enable only if you need cookies/credentials
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Enable CORS BEFORE authorization and before mapping controllers
            app.UseCors("AllowBlazorClient");

            app.UseAuthorization();


            app.MapControllers();
            
            // Seed DB from movies.json
            using (var scope = app.Services.CreateScope())  // Create a scope to get scoped services like DbContext
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();  // Get the AppDbContext instance

                if (!db.Movies.Any())   // Check if the Movies table is empty
                {
                    var jsonFile = Path.Combine(app.Environment.ContentRootPath, "Data", "movies.json");    // Path to movies.json file

                    if (File.Exists(jsonFile))  // Check if the file exists
                    {
                        var json = File.ReadAllText(jsonFile);  // Read the file content
                        var movies = JsonSerializer.Deserialize<List<Movie>>(json); // Deserialize JSON to List<Movie>

                        if (movies != null && movies.Count > 0)
                        {
                            db.Movies.AddRange(movies); // Add movies to the DbContext
                            db.SaveChanges();   // Save changes to the in-memory database
                        }
                    }
                }
            }

            app.Run();
        }
    }
}
