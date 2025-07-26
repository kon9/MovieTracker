using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieTracker.Infrastructure.Interfaces;
using MovieTracker.Models;
using MovieTracker.Models.Dto;
using MovieTracker.Models.ViewModels;

namespace MovieTracker.Controllers;


[Authorize]
[ApiController]
[Route("movies")]
public class MoviesController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMovieService _movieService;

    public MoviesController(UserManager<AppUser> userManager, IMovieService movieService)
    {
        _userManager = userManager;
        _movieService = movieService;
    }

    [HttpGet()]
    public async Task<ActionResult<IEnumerable<MovieVm>>> GetMovies()
    {
        var moviesVm = await _movieService.GetAllMoviesAsync();
        return Ok(moviesVm);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MovieVm>> GetMovie(int movieId)
    {
        var movieVm = await _movieService.GetMovieAsync(movieId);
        if (movieVm == null) return NotFound();
        return Ok(movieVm);
    }

    [HttpPost()]
    public async Task<ActionResult<MovieVm>> CreateMovie(MovieDto movieDto)
    {
        var movieVm = await _movieService.CreateMovieAsync(movieDto);
        return CreatedAtAction(nameof(GetMovie), new { id = movieVm.Id }, movieVm);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateMovie(int movieId, MovieDto movieDto)
    {
        await _movieService.UpdateMovieAsync(movieId, movieDto);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteMovie(int movieId)
    {
        await _movieService.DeleteMovieAsync(movieId);
        return NoContent();
    }

    [HttpPost("{id:int}/rating")]
    public async Task<IActionResult> RateMovie(int movieId, RatingDto ratingDto)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var rating = await _movieService.RateMovieAsync(movieId, ratingDto, user.Id);
        return CreatedAtAction("RateMovie", new { id = rating.Id }, rating);
    }
}

