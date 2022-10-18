using Microsoft.AspNetCore.Identity;

namespace Watchlist.Data.Models
{
    public class User : IdentityUser
    {
        public List<UserMovie> UsersMovies { get; set; } = new List<UserMovie>();
    }
}
//•	Has an Id – a string, Primary Key
//•	Has a Username – a string with min length 5 and max length 20 (required)
//•	Has an Email – a string with min length 10 and max length 60 (required)
//•	Has a Password – a string with min length 5 and max length 20 (before hashed) – no max length required for a hashed password in the database (required)
//•	Has UsersMovies – a collection of type UserMovie
