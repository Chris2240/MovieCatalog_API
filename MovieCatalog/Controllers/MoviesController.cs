using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MovieCatalog.Models;
using System.Text.Json;

namespace MovieCatalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        /*        
            ---------------------------------------------

        // Phase 1 "LINQ + JSON" which is still works after "EF Core" setup only without transition yet (keep for references)
        
            ---------------------------------------------
        * without EF Core - start debug "F5" as "http"
        * with EF Core -    "dotnet run" from terminal        

        */

        // Load movies from a JSON file
        private List<Movie> LoadMovies()
        {
            // Read the JSON file content
            var jsonData = System.IO.File.ReadAllText("Data/movies.json");

            // Deserialize the JSON data into a list of movies = convert JSON → object
            return JsonSerializer.Deserialize<List<Movie>>(jsonData) ?? new List<Movie>();
        }

        // GET: api/Movies
        // Returns all movies
        [HttpGet]
        public ActionResult GetAllMovies()
        {
            var movies = LoadMovies();
            return Ok(movies);
        }

        // GET: api/Movies/top-rated
        // Returns top-rated movies, default is 5
        [HttpGet("top-rated")]
        public ActionResult GetTopMovie(int count = 5)
        {
            var movie = LoadMovies();
            // Get top 'count' movies by rating in descending order
            var top = movie.OrderByDescending(m => m.Rating).Take(count);
            return Ok(top);
        }

        // GET: api/Movies/rating
        // Returns movies by providing rating number
        [HttpGet("rating")]
        public ActionResult GetRated(double rate_provide) 
        {
            // Load all movies
            var movies = LoadMovies();
            // Filter movies by the specified rating
            var rating = movies.Where(m => m.Rating.Equals(rate_provide));
            return Ok(rating);
        }

        // GET: api/Movies/descending order by year
        // Returns movies in descending order by year
        [HttpGet("descending order by year")]
        public ActionResult GetByYear()
        {
            var movies = LoadMovies();
            var filtered = movies.OrderByDescending(m => m.Year);
            return Ok(filtered);
        }

        // GET: api/Movies/genre/{genre}
        // Returns movies by genre
        [HttpGet("genre/{genre}")]      // "../{genre}" - means required parameter
        public ActionResult GetByGenre(string genre)
        {
            var movies = LoadMovies();
            // Case-insensitive comparison for genre - an example "AcTion" == "action"
            var filtered = movies.Where(m => m.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase));
            return Ok(filtered);
        }

    }
}
