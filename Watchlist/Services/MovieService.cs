using Microsoft.EntityFrameworkCore;
using Watchlist.Contracts;
using Watchlist.Data;
using Watchlist.Data.Models;
using Watchlist.Models;

namespace Watchlist.Services
{
    public class MovieService : IMovieService
    {
        private readonly WatchlistDbContext context;

        public MovieService(WatchlistDbContext _context)
        {
            context = _context;
        }

        public async Task AddMovieAsync(AddMovieViewModel model)
        {
            var entity = new Movie()
            {
                Director = model.Director,
                Title = model.Title,
                GenreId = model.GenreId,
                ImageUrl = model.ImageUrl,
                Rating = model.Rating
            };

            await context.Movies.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task AddMovieToCollectionAsync(int movieId, string userId)
        {

            var user = await context.Users
                 .Where(u => u.Id == userId)
                 .Include(u => u.UsersMovies)
                 .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException("Invalid UserId");
            }

            var movie = await context.Movies.FirstOrDefaultAsync(u => u.Id == movieId);

            if (movie == null)
            {
                throw new ArgumentException("Invalid Movie Id");

            }

            if (!user.UsersMovies.Any(m => m.MovieId == movieId))
            {
                user.UsersMovies.Add(new UserMovie()
                {
                    MovieId = movie.Id,
                    UserId = user.Id,
                    Movie = movie,
                    User = user
                });

                await context.SaveChangesAsync();
            }
           
        }

        public async Task EditAsync(EditMovieViewModel model)
        {
            var entity = await context.Movies.FindAsync(model.Id);

            entity.Rating = model.Rating;
            entity.ImageUrl = model.ImageUrl;
            entity.Director = model.Director;
            entity.Title = model.Title;
            entity.GenreId = model.GenreId;

            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<MovieViewModel>> GetAllAsync()
        {
            var entities = await context.Movies.ToListAsync();


            return entities
                .Select(m => new MovieViewModel()
                {
                    Id = m.Id,
                    Director = m.Director,
                    Genre = m?.Genre?.Name,
                    ImageUrl = m.ImageUrl,
                    Rating = m.Rating,
                    Title = m.Title
                });
        }

        public async Task<EditMovieViewModel> GetForEditAsync(int id)
        {
            var movie = await context.Movies.FindAsync(id);

            var model =  new EditMovieViewModel()
            {
                Id = id,
                Director = movie.Director,
                GenreId = movie.GenreId ?? -1,
                ImageUrl = movie.ImageUrl,
                Rating = movie.Rating,
                Title = movie.Title
            };

            model.Genres = await GetGenresAsync();

            return model;
        }

        public async Task<IEnumerable<Genre>> GetGenresAsync()
        {
            return await context.Genres.ToListAsync();
        }

        public async Task<IEnumerable<MovieViewModel>> GetWatchedAsync(string userId)
        {
            var user = await context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.UsersMovies)
                .ThenInclude(um => um.Movie)
                .ThenInclude(m => m.Genre)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException("Invalid UserId");
            }

            return user.UsersMovies.Select(m => new MovieViewModel()
            {
                Director = m.Movie.Director,
                Genre = m.Movie.Genre?.Name,
                Id = m.MovieId,
                ImageUrl = m.Movie.ImageUrl,
                Title = m.Movie.Title,
                Rating = m.Movie.Rating

            });

        }

        public async Task RemoveMovieFormCollectionAsync(int movieId, string userId)
        {
            var user = await context.Users
                 .Where(u => u.Id == userId)
                 .Include(u => u.UsersMovies)
                 .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException("Invalid UserId");
            }

            var movie = user.UsersMovies.FirstOrDefault(m => m.MovieId == movieId);

            if (movie != null)
            {
                user.UsersMovies.Remove(movie);

                await context.SaveChangesAsync();
            }

        }
    }
}
