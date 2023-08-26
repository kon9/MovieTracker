using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MovieTracker.Infrastructure.Data;
using MovieTracker.Infrastructure.Interfaces;
using MovieTracker.Models;

namespace MovieTracker.Infrastructure.Repo;

public class CommentsRepository : Repository<Comment>, ICommentsRepository
{
    public CommentsRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<int> GetRatingForComment(int commentId)
    {
        var comment = await _context.Comments.FindAsync(commentId);
        if (comment == null)
            throw new ArgumentException("Comment not found.");

        return comment.TotalRating;
    }

    public async Task<IEnumerable<Comment>> GetAllCommentsForMovieAsync(int movieId)
    {
        return await _context.Comments
            .Where(c => c.MovieId == movieId)
            .Include(c => c.AppUser)  // Load associated AppUser.
            .ToListAsync();
    }

    public async Task UpdateComment(Comment comment)
    {
        if (comment == null)
            throw new ArgumentNullException(nameof(comment));

        var existingComment = await _context.Comments.FindAsync(comment.Id);
        if (existingComment == null)
            throw new ArgumentException("Comment does not exist.");

        existingComment.Content = comment.Content;

        await _context.SaveChangesAsync();
    }

    public async Task UpVoteComment(int commentId, string appUserId)
    {
        var comment = await _context.Comments.FindAsync(commentId);
        if (comment == null)
            throw new ArgumentException("Comment not found.");

        var existingRating = await _context.CommentRatings
            .FirstOrDefaultAsync(r => r.CommentId == commentId && r.AppUserId == appUserId);

        if (existingRating != null)
        {
            if (existingRating.IsUpvote)
                return;
            else
            {
                existingRating.IsUpvote = true;
                comment.TotalRating += 2;
            }
        }
        else
        {
            _context.CommentRatings.Add(new CommentRating
            {
                CommentId = commentId,
                AppUserId = appUserId,
                IsUpvote = true
            });
            comment.TotalRating++;
        }

        await _context.SaveChangesAsync();
    }

    public async Task DownVoteComment(int commentId, string appUserId)
    {
        var comment = await _context.Comments.FindAsync(commentId);
        if (comment == null)
            throw new ArgumentException("Comment not found.");

        var existingRating = await _context.CommentRatings
            .FirstOrDefaultAsync(r => r.CommentId == commentId && r.AppUserId == appUserId);

        if (existingRating != null)
        {
            if (!existingRating.IsUpvote)
                return;
            else
            {
                existingRating.IsUpvote = false;
                comment.TotalRating -= 2;
            }
        }
        else
        {
            _context.CommentRatings.Add(new CommentRating
            {
                CommentId = commentId,
                AppUserId = appUserId,
                IsUpvote = false
            });
            comment.TotalRating--;
        }

        await _context.SaveChangesAsync();
    }
}
