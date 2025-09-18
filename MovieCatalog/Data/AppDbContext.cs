using Microsoft.EntityFrameworkCore;
using MovieCatalog.Models;

namespace MovieCatalog.Data
{
    public class AppDbContext : DbContext   // Inherit from DbContext class
    {
        // Constructor that takes DbContextOptions and passes it to the base class constructor
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Define a DbSet for the Movie entity (the Movie class represents a database table in the model)
        public DbSet<Movie> Movies { get; set; } = default!;    // DbSet for Movie entity (Movie class)
    }
}
