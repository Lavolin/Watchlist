using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Watchlist.Data.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string Director { get; set; } = null!;

        [Required]
        public string ImageUrl { get; set; } = null!;

        [Required]
        public decimal Rating { get; set; }


        [ForeignKey(nameof(Genre))]
        public int? GenreId { get; set; }

        public Genre? Genre { get; set; }

        public List<UserMovie> UsersMovies { get; set; } = new List<UserMovie>();
    }
}

//• Has Id – a unique integer, Primary Key
//•	Has Title – a string with min length 10 and max length 50 (required)
//•	Has Director – a string with min length 5 and max length 50 (required)
//•	Has ImageUrl – a string (required)
//•	Has Rating – a decimal with min value 0.00 and max value 10.00 (required)
//•	Has GenreId – an integer
//•	Has Genre – a Genre 
//•	Has UsersMovies – a collection of type UserMovie
