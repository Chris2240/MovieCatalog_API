namespace MovieCatalog.Models
{
    public class Movie
    {
        // Properties of the Movie class
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Genre { get; set; } = "";
        public int Year { get; set; }
        public double Rating { get; set; }  // e.g., 4.5 rating
    }
}
