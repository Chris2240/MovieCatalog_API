using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCatalog.Data;
using MovieCatalog.Models;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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

        /*
        
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

        */

        /*
            ---------------------------------------------
        
            Phase 2b - Transition JSON to EF Core:

            ---------------------------------------------
        */

        // Inject AppDbContext to interact with the database in the controller
        private readonly AppDbContext _dbContext;

        // Constructor to initialize the AppDbContext for the controller class
        public MoviesController(AppDbContext dbContext)   // Dependency Injection of AppDbContext in the constructor
        {
            this._dbContext = dbContext;
        }

        // GET: api/Movies
        // Returns all movies
        [HttpGet]
        public ActionResult GetAllMovies()
        {
            var movies = _dbContext.Movies.ToList();  // Retrieve all movies from the database and convert to list
            return Ok(movies);
        }

        // GET: api/Movies/top-rated
        // Returns top-rated movies, default is 1
        [HttpGet("top-rated")]
        public ActionResult GetTopMovie(int count = 1)
        {
            // Get top 'count' movies by rating in descending order
            var top = _dbContext.Movies.OrderByDescending(m => m.Rating)
                .Take(count)
                .ToList();      // Execute the query and convert to list - VERRY IMPORTANT: the query is executed inside your controller

            return Ok(top);
        }

        // GET: api/Movies/rating
        [HttpGet("rating")]
        public ActionResult GetRated(double rate_provide)
        {
            // Filter movies by the specified rating
            var rating = _dbContext.Movies
                .Where(m => m.Rating.Equals(rate_provide))
                .ToList();      // Execute the query and convert to list

            return Ok(rating);
        }

        // GET: api/Movies/descending-order-by-year
        // Returns Movies in descending order by year
        [HttpGet("descending-order-by-year")]
        public ActionResult GetByYear()
        {
            var filteredByDecending = _dbContext.Movies
                .OrderByDescending(m => m.Year)
                .ToList();

            return Ok(filteredByDecending);
        }

        // GET: api/Movies/genre/{genre}
        // Returns movies by genre
        [HttpGet("genre/{genre}")]  // "../{genre}" - means required parameter
        public ActionResult GetByGenre(string genre)
        {
            var filtredByGenre = _dbContext.Movies
                .Where(m => m.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase)) // Case-insensitive comparison for genre - an example "AcTion" == "action"
                .ToList();

            return Ok(filtredByGenre);
        }

        // Extra: "Select" query purpose and "BadRequest" custom message example
        // GET: api/Movies/id/title/genre/{id_selected}
        // Returns Movie id, title and genre
        [HttpGet("id/title/genre/{id_selected}")]
        public ActionResult GetTitle(int id_selected)
        {
            var allMovies = _dbContext.Movies.ToList();     // Get all movies from the database

            // Check if the provided id_selected exists in the database and return only if it exists otherwise return BadRequest custom message with available Id's only
            // "if (id_selected != allMovies.FirstOrDefault(m => m.Id == id_selected)?.Id)" - works but nullable check alternative
            if (!allMovies.Any(m => m.Id == id_selected))
            {

                return BadRequest($"Invalid Id. Please select the available Id's from the following: {string.Join(", ", allMovies.Select(m => m.Id))}");

            }
            else
            {
                // Use LINQ to filter and select only Id, Title and Genre fields
                var filteredByTitle = _dbContext.Movies
                .Where(m => m.Id.Equals(id_selected))
                .Select(m => new { m.Id, m.Title, m.Genre })    // Select only Id, Title and Genre fields
                .ToList();

                return Ok(filteredByTitle);
            }
        }


        /*         
            ---------------------------------------------

            Phase 2c - Add CRUD Endpoints

            ---------------------------------------------
        */

        /*
         1. Create - POST
         2. Read - GET (already implemented above)
        // POST: api/Movies
        // Create new Movie
        [HttpPost("create-new-movie")]
        public ActionResult CreateMovie(Movie newMovie)
        {
            _dbContext.Movies.Add(newMovie);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetAllMovies), new { id = newMovie.Id }, newMovie);
        }
        */

        // POST: api/Movies
        // Create new Movie - Upgraded version (with validation)
        [HttpPost("create-new-movie")]
        public ActionResult CreateMovie(Movie newMovie)
        {
            // Ensure Id is non-negative and not equal to 0.
            if (newMovie.Id <= 0)
            {
                return BadRequest("Invalid Id. Id must be greater and not equal to 0.");
            }

            // Check if a movie with this Id already exists
            if (_dbContext.Movies.Any(m => m.Id == newMovie.Id))
            {
                return BadRequest($"The movie with Id: '{newMovie.Id}' already exists.");
            }

            // Save new movie to the database
            _dbContext.Movies.Add(newMovie);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetAllMovies), new { id = newMovie.Id }, newMovie);
        }

        // PUT: api/Movies/{id}
        // Update Movie
        [HttpPut("update_movie/{id}")]
        public ActionResult UpdateMovie(int id, Movie updateMovie)
        {
            var existingMovie = _dbContext.Movies.Find(id);

            // if movie not exist return not found
            if (existingMovie == null)
            {
                return NotFound();
            }

            // Only update fields if they are provided (not default/null/empty)
            if (!string.IsNullOrWhiteSpace(updateMovie.Title) && updateMovie.Title != "string" || updateMovie.Title == string.Empty) // Accept only a non-empty, non-whitespace and non-default "string" title or empty string(null)
                existingMovie.Title = updateMovie.Title;

            if (!string.IsNullOrWhiteSpace(updateMovie.Genre) && updateMovie.Genre != "string" || updateMovie.Genre == string.Empty)
                existingMovie.Genre = updateMovie.Genre;

            if (updateMovie.Year != default)
                existingMovie.Year = updateMovie.Year;

            if (updateMovie.Rating != default)
                existingMovie.Rating = updateMovie.Rating;

            _dbContext.SaveChanges();
            return Ok(existingMovie);
        }

        // DELETE: api/Movies/{id}
        // Delete Movie using "Id"
        [HttpDelete("delete_movie/{id}")] //  "/" added before "{id}" to avoid route conflict with other GET endpoints
        public ActionResult DeleteMovie(int id)
        {
            var findMovie = _dbContext.Movies.Find(id);     // Find the movie by ID

            if (findMovie == null)
            {
                return NotFound();
            }

            _dbContext.Movies.Remove(findMovie);
            _dbContext.SaveChanges();

            return NoContent();     // Standard response for successful DELETE requests
        }
    }
}
