using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTracker.Data;
using MovieTracker.Models.Dto;
using System.Security.Claims;

namespace MovieTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RatingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RatingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Ratings
        [HttpGet("{id}")]
        public async Task<IEnumerable<Rating>> GetRating(int id)
        {
            return await _context.Ratings.Where(r => r.MovieId == id).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<RatingDto>> PostRating(RatingDto ratingDto)
        {
            var movie = await _context.Movies.FindAsync(ratingDto.MovieId);

            if (movie == null)
            {
                return NotFound();
            }

            // Get UserId from the token (which we added as a claim in the AuthController)
            string userId = User.FindFirstValue("id");

            // Check if the user has already rated this movie
            var existingRating = await _context.Ratings
                .Where(r => r.MovieId == ratingDto.MovieId && r.AppUserId == userId)
                .FirstOrDefaultAsync();

            if (existingRating != null)
            {
                return BadRequest("You have already rated this movie");
            }

            var score = ratingDto.Score switch
            {
                < 0 => 0,
                > 10 => 10,
                _ => ratingDto.Score
            };

            var rating = new Rating
            {
                Score = score,
                Movie = movie,
                AppUserId = userId // Set the user who is making the rating
            };

            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();

            return CreatedAtAction("PostRating", new { id = rating.Id }, ratingDto);
        }
    }
}
