using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Watchlist.Data.Models
{
    public class UserMovie
    {
        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public User User { get; set; }

        [Required]
        [ForeignKey(nameof(Movie))]
        public int MovieId { get; set; }

        public Movie Movie { get; set; }
    }
}
//•	UserId – string, Primary Key, foreign key (required)
//•	User – User
//•	MovieId – integer, Primary Key, foreign key (required)
//•	Movie – Movie
