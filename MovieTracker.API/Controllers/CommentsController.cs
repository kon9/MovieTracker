using Microsoft.AspNetCore.Mvc;
using MovieTracker.Core.DTOs;
using MovieTracker.Core.Services;

namespace MovieTracker.API.Controllers
{
    public class CommentsController : BaseApiController
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments()
        {
            try
            {
                var comments = await _commentService.GetAllAsync();
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CommentDto>> GetComment(int id)
        {
            try
            {
                var comment = await _commentService.GetByIdAsync(id);
                return HandleResult(comment, "Comment not found");
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("movie/{movieId}")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentsByMovie(int movieId)
        {
            try
            {
                var comments = await _commentService.GetByMovieIdAsync(movieId);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentsByUser(int userId)
        {
            try
            {
                var comments = await _commentService.GetByUserIdAsync(userId);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult<CommentDto>> CreateComment(CreateCommentDto createCommentDto)
        {
            try
            {
                // In a real application, you would get the user ID from the JWT token
                int userId = 1; // Placeholder - should come from authentication
                var comment = await _commentService.CreateAsync(userId, createCommentDto);
                return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, comment);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CommentDto>> UpdateComment(int id, UpdateCommentDto updateCommentDto)
        {
            try
            {
                // In a real application, you would get the user ID from the JWT token
                int userId = 1; // Placeholder - should come from authentication
                var comment = await _commentService.UpdateAsync(id, userId, updateCommentDto);
                return Ok(comment);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteComment(int id)
        {
            try
            {
                // In a real application, you would get the user ID from the JWT token
                int userId = 1; // Placeholder - should come from authentication
                await _commentService.DeleteAsync(id, userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
} 