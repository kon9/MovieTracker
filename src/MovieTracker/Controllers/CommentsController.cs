using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieTracker.Infrastructure.Interfaces;
using MovieTracker.Models;
using MovieTracker.Models.Dto;

namespace MovieTracker.Controllers;

[Authorize]
[ApiController]
[Route("movies/{movieId:int}/comments")]
public class CommentsController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ICommentService _commentService;

    public CommentsController(UserManager<AppUser> userManager, ICommentService commentService)
    {
        _userManager = userManager;
        _commentService = commentService;
    }

    [HttpPost]
    public async Task<IActionResult> AddComment(int movieId, CommentDto commentDto)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        var result = await  _commentService.AddCommentAsync(movieId, new CommentDto { Content = commentDto.Content }, user.Id);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetComments(int movieId)
    {
        var comments = await _commentService.GetCommentsForMovieAsync(movieId);
        return Ok(comments);
    }

    [HttpGet("{commentId:int}")]
    public async Task<IActionResult> GetComment(int movieId, int commentId)
    {
        var comment = await _commentService.GetCommentAsync(commentId);
        if (comment is null) return NotFound();
        return Ok(comment);
    }

    [HttpPost("{commentId:int}/upvote")]
    public async Task<IActionResult> UpVoteComment(int commentId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        var result = await _commentService.UpVoteCommentAsync(commentId, user.Id);

        return Ok(result);
    }

    [HttpPost("{commentId:int}/downvote")]
    public async Task<IActionResult> DownVoteComment(int commentId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        var result = await _commentService.DownVoteCommentAsync(commentId, user.Id);

        return Ok(result);
    }

    [HttpPost("{commentId:int}/reply")]
    public async Task<IActionResult> ReplyToComment(int commentId, CommentDto commentDto)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        var result = await _commentService.ReplyToCommentAsync(commentId, new CommentDto { Content = commentDto.Content },user.Id);

        return Ok(result);
    }
}

