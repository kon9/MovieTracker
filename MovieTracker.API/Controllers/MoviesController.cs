using Microsoft.AspNetCore.Mvc;
using MovieTracker.Core.DTOs;
using MovieTracker.Core.Services;

namespace MovieTracker.API.Controllers
{
    public class MoviesController : BaseApiController
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies()
        {
            try
            {
                var movies = await _movieService.GetAllAsync();
                return Ok(movies);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDto>> GetMovie(int id)
        {
            try
            {
                var movie = await _movieService.GetByIdAsync(id);
                return HandleResult(movie, "Movie not found");
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<MovieDto>>> SearchMovies([FromQuery] string searchTerm)
        {
            try
            {
                var movies = await _movieService.SearchAsync(searchTerm);
                return Ok(movies);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("genre/{genre}")]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMoviesByGenre(string genre)
        {
            try
            {
                var movies = await _movieService.GetByGenreAsync(genre);
                return Ok(movies);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("year/{year}")]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMoviesByYear(int year)
        {
            try
            {
                var movies = await _movieService.GetByYearAsync(year);
                return Ok(movies);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult<MovieDto>> CreateMovie(CreateMovieDto createMovieDto)
        {
            try
            {
                var movie = await _movieService.CreateAsync(createMovieDto);
                return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MovieDto>> UpdateMovie(int id, UpdateMovieDto updateMovieDto)
        {
            try
            {
                var movie = await _movieService.UpdateAsync(id, updateMovieDto);
                return Ok(movie);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMovie(int id)
        {
            try
            {
                await _movieService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
} 