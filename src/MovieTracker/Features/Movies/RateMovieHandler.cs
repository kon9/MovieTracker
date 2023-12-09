using MediatR;
using MovieTracker.Infrastructure.Interfaces;
using MovieTracker.Models;
using MovieTracker.Models.Dto;
using MovieTracker.Models.ViewModels;

namespace MovieTracker.Features.Reviews;

public record RateMovieCommand(int Id, RatingDto RatingDto, string AppUserId) : IRequest<RatingVm>;

public class RateMovieHandler : IRequestHandler<RateMovieCommand, RatingVm>
{
    private readonly IMovieService _movieService;

    public RateMovieHandler(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public async Task<RatingVm> Handle(RateMovieCommand request, CancellationToken cancellationToken)
    {
        return await _movieService.RateMovieAsync(request.Id, request.RatingDto, request.AppUserId);
    }
}
