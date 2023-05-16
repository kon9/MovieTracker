using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTracker.Data;
using MovieTracker.Models;
using MovieTracker.Models.Dto;

namespace MovieTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies()
        {
            var movies = await _context.Movies
                .Include(m => m.Ratings)
                .ToListAsync();

            var movieDtos = movies.Select(m => new MovieDto
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                ImageUrl = m.ImageUrl,
                Ratings = m.Ratings.Select(r => new RatingDto
                {
                    Id = r.Id,
                    Score = r.Score,
                    MovieId = r.MovieId
                }).ToList()
            }).ToList();

            return movieDtos;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDto>> GetSingleMovie(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.Ratings)
                .Include(m => m.Comments)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            var movieDto = new MovieDto
            {
                Id = movie.Id,
                Name = movie.Name,
                Description = movie.Description,
                ImageUrl = movie.ImageUrl,
                Ratings = movie.Ratings.Select(r => new RatingDto
                {
                    Id = r.Id,
                    Score = r.Score,
                    MovieId = r.MovieId
                }).ToList(),
                Comments = movie.Comments.Select(c => new CommentDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    MovieId = c.MovieId
                }).ToList()
            };

            return movieDto;
        }


        [HttpPost]
        public async Task<ActionResult<MovieDto>> PostMovie(MovieDto movieDto)
        {
            var movie = new Movie
            {
                Name = movieDto.Name,
                Description = movieDto.Description,
                ImageUrl = movieDto.ImageUrl
            };

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("PostMovie", new { id = movie.Id }, movieDto);
        }
    }
}
