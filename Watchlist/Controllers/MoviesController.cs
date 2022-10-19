using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Watchlist.Contracts;
using Watchlist.Models;

namespace Watchlist.Controllers
{
    [Authorize]
    public class MoviesController : Controller
    {
        private readonly IMovieService movieService;

        public MoviesController(IMovieService _movieService)
        {
           movieService = _movieService;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var model = await movieService.GetAllAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddMovie()
        {
            var model = new AddMovieViewModel()
            {
                Genres = await movieService.GetGenresAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddMovie(AddMovieViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await movieService.AddMovieAsync(model);

                return RedirectToAction(nameof(All));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "You did an Error!!!");

                return View(model);
            }
        }
    }
}
