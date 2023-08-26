using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieTracker.Features.Comments;
using MovieTracker.Features.Movies;
using MovieTracker.Models;
using MovieTracker.Models.Dto;
using MovieTracker.Models.ViewModels;

namespace MovieTracker.Controllers;
//hate it
[Route("Movies/{movieId:int}/comments")]
[ApiController]
[Authorize]
public class CommentsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly UserManager<AppUser> _userManager;

    public CommentsController(IMediator mediator, UserManager<AppUser> userManager)
    {
        _userManager = userManager;
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> AddComment(int movieId, CommentDto commentDto)
    {
        var user = await _userManager.GetUserAsync(User);
        foreach (var claim in User.Claims)
        {
            Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
        }
        if (user == null)
            return Unauthorized();

        var command = new AddCommentCommand(movieId, commentDto.Content, user.Id);

        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetComments(int movieId)
    {
        var comments = await _mediator.Send(new GetCommentsQuery(movieId));
        return Ok(comments);
    }

    [HttpGet("{commentId:int}")]
    public async Task<IActionResult> GetComment(int movieId, int commentId)
    {
        var comment = await _mediator.Send(new GetCommentQuery(commentId));
        if (comment == null) return NotFound();
        return Ok(comment);
    }

    [HttpPost("{commentId:int}/upvote")]
    public async Task<IActionResult> UpVoteComment(int commentId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        var command = new UpVoteCommentCommand(commentId, user.Id);

        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpPost("{commentId:int}/downvote")]
    public async Task<IActionResult> DownVoteComment(int commentId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        var command = new DownVoteCommentCommand(commentId, user.Id);

        var result = await _mediator.Send(command);

        return Ok(result);
    }
}

