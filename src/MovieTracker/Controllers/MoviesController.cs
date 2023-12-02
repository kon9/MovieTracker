using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieTracker.Infrastructure.Interfaces;
using MovieTracker.Models;
using MovieTracker.Models.Dto;
using MovieTracker.Models.ViewModels;
using System.Security.Claims;
using MovieTracker.Features.Comments;
using MovieTracker.Features.Movies;
using MovieTracker.Features.Reviews;

namespace MovieTracker.Controllers;


[Authorize]
[ApiController]
[Route("movies")]
public class MoviesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly UserManager<AppUser> _userManager;

    public MoviesController(IMediator mediator, UserManager<AppUser> userManager)
    {
        _userManager = userManager;
        _mediator = mediator;
    }

    [HttpGet()]
    public async Task<ActionResult<IEnumerable<MovieVm>>> GetMovies()
    {
        var moviesVm = await _mediator.Send(new GetMoviesQuery());
        return Ok(moviesVm);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MovieVm>> GetMovie(int id)
    {
        var movieVm = await _mediator.Send(new GetMovieQuery(id));
        if (movieVm == null) return NotFound();
        return Ok(movieVm);
    }

    [HttpPost()]
    public async Task<ActionResult<MovieVm>> CreateMovie(MovieDto movieDto)
    {
        var movieVm = await _mediator.Send(new CreateMovieCommand(movieDto));
        return CreatedAtAction(nameof(GetMovie), new { id = movieVm.Id }, movieVm);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateMovie(int id, MovieDto movieDto)
    {
        await _mediator.Send(new UpdateMovieCommand(id, movieDto));
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        await _mediator.Send(new DeleteMovieCommand(id));
        return NoContent();
    }

    [HttpPost("{id:int}/rating")]
    public async Task<IActionResult> RateMovie(int id, RatingDto ratingDto)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var rating = await _mediator.Send(new RateMovieCommand(id, ratingDto, user.Id));
        return CreatedAtAction("RateMovie", new { id = rating.Id }, rating);//returns appuser model BUG
    }

    [HttpPut("{id:int}/rating")]
    public async Task<IActionResult> UpdateRating(int id, RatingDto ratingDto)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        await _mediator.Send(new UpdateRatingCommand(id, ratingDto, user.Id));
        return NoContent();
    }
}

