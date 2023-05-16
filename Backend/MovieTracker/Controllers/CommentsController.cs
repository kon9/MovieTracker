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
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Comments
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments(int id)
        {
            return await _context.Comments.Where(c => c.MovieId == id).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<CommentDto>> PostComment(CommentDto commentDto)
        {
            var movie = await _context.Movies.FindAsync(commentDto.MovieId);

            if (movie == null)
            {
                return NotFound();
            }

            var comment = new Comment
            {
                Content = commentDto.Content,
                Movie = movie
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("PostComment", new { id = comment.Id }, commentDto);
        }
    }
}
