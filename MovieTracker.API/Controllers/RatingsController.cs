using Microsoft.AspNetCore.Mvc;
using MovieTracker.Core.DTOs;
using MovieTracker.Core.Services;

namespace MovieTracker.API.Controllers
{
    public class RatingsController : BaseApiController
    {
        private readonly IRatingService _ratingService;

        public RatingsController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RatingDto>>> GetRatings()
        {
            try
            {
                var ratings = await _ratingService.GetAllAsync();
                return Ok(ratings);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RatingDto>> GetRating(int id)
        {
            try
            {
                var rating = await _ratingService.GetByIdAsync(id);
                return HandleResult(rating, "Rating not found");
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("movie/{movieId}")]
        public async Task<ActionResult<IEnumerable<RatingDto>>> GetRatingsByMovie(int movieId)
        {
            try
            {
                var ratings = await _ratingService.GetByMovieIdAsync(movieId);
                return Ok(ratings);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<RatingDto>>> GetRatingsByUser(int userId)
        {
            try
            {
                var ratings = await _ratingService.GetByUserIdAsync(userId);
                return Ok(ratings);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("movie/{movieId}/average")]
        public async Task<ActionResult<double>> GetAverageRating(int movieId)
        {
            try
            {
                var averageRating = await _ratingService.GetAverageRatingAsync(movieId);
                return Ok(averageRating);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("movie/{movieId}/total")]
        public async Task<ActionResult<int>> GetTotalRatings(int movieId)
        {
            try
            {
                var totalRatings = await _ratingService.GetTotalRatingsAsync(movieId);
                return Ok(totalRatings);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("user/{userId}/movie/{movieId}")]
        public async Task<ActionResult<RatingDto>> GetUserRatingForMovie(int userId, int movieId)
        {
            try
            {
                var rating = await _ratingService.GetByUserAndMovieAsync(userId, movieId);
                return HandleResult(rating, "Rating not found");
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult<RatingDto>> CreateRating(CreateRatingDto createRatingDto)
        {
            try
            {
                // In a real application, you would get the user ID from the JWT token
                int userId = 1; // Placeholder - should come from authentication
                var rating = await _ratingService.CreateAsync(userId, createRatingDto);
                return CreatedAtAction(nameof(GetRating), new { id = rating.Id }, rating);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("movie/{movieId}")]
        public async Task<ActionResult<RatingDto>> UpdateRating(int movieId, UpdateRatingDto updateRatingDto)
        {
            try
            {
                // In a real application, you would get the user ID from the JWT token
                int userId = 1; // Placeholder - should come from authentication
                var rating = await _ratingService.UpdateAsync(userId, movieId, updateRatingDto);
                return Ok(rating);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpDelete("movie/{movieId}")]
        public async Task<ActionResult> DeleteRating(int movieId)
        {
            try
            {
                // In a real application, you would get the user ID from the JWT token
                int userId = 1; // Placeholder - should come from authentication
                await _ratingService.DeleteAsync(userId, movieId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
} 