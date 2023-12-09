using AutoMapper;
using MediatR;
using MovieTracker.Infrastructure.Interfaces;
using MovieTracker.Models.ViewModels;
using MovieTracker.Models;
using MovieTracker.Models.Dto;

namespace MovieTracker.Features.Movies;

public record CreateMovieCommand(MovieDto MovieDto) : IRequest<MovieVm>;

public class CreateMovieHandler : IRequestHandler<CreateMovieCommand, MovieVm>
{
    private readonly IMovieService _movieService;

    public CreateMovieHandler(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public async Task<MovieVm> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
    {
        return await _movieService.CreateMovieAsync(request.MovieDto);
    }
}