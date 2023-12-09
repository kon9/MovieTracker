using AutoMapper;
using MediatR;
using MovieTracker.Infrastructure.Interfaces;
using MovieTracker.Models.ViewModels;

namespace MovieTracker.Features.Movies;

public record GetMovieQuery(int Id) : IRequest<MovieVm>;

public class GetMovieHandler : IRequestHandler<GetMovieQuery, MovieVm>
{
    private readonly IMovieService _movieService;

    public GetMovieHandler(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public async Task<MovieVm> Handle(GetMovieQuery request, CancellationToken cancellationToken)
    {
        return await _movieService.GetMovieAsync(request.Id);
    }
}