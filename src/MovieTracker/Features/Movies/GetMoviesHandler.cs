using AutoMapper;
using MediatR;
using MovieTracker.Infrastructure.Interfaces;
using MovieTracker.Models.ViewModels;

namespace MovieTracker.Features.Movies;

public record GetMoviesQuery() : IRequest<IEnumerable<MovieVm>>;

public class GetMoviesHandler : IRequestHandler<GetMoviesQuery, IEnumerable<MovieVm>>
{
    private readonly IMovieService _movieService;

    public GetMoviesHandler(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public async Task<IEnumerable<MovieVm>> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
    {
        return await _movieService.GetAllMoviesAsync();
    }
}