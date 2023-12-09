using AutoMapper;
using MediatR;
using MovieTracker.Infrastructure.Interfaces;
using MovieTracker.Models;
using MovieTracker.Models.Dto;

namespace MovieTracker.Features.Movies;


public record UpdateMovieCommand(int Id, MovieDto MovieDto) : IRequest<Unit>;
public class UpdateMovieHandler : IRequestHandler<UpdateMovieCommand, Unit>
{
    private readonly IMovieService _movieService;

    public UpdateMovieHandler(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public async Task<Unit> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        await _movieService.UpdateMovieAsync(request.Id, request.MovieDto);
        return Unit.Value;
    }
}