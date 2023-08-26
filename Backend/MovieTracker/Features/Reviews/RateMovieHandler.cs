using MediatR;
using MovieTracker.Infrastructure.Interfaces;
using MovieTracker.Models;
using MovieTracker.Models.Dto;

namespace MovieTracker.Features.Reviews;

public record RateMovieCommand(int Id, RatingDto RatingDto) : IRequest<Rating>;

public class RateMovieHandler : IRequestHandler<RateMovieCommand, Rating>
{
    private readonly IMoviesRepository _moviesRepository;

    public RateMovieHandler(IMoviesRepository moviesRepository)
    {
        _moviesRepository = moviesRepository;
    }

    public async Task<Rating> Handle(RateMovieCommand request, CancellationToken cancellationToken)
    {
        var rating = new Rating { Score = request.RatingDto.Score, MovieId = request.Id };
        await _moviesRepository.AddRatingToMovie(request.Id, rating);
        return rating;
    }
}
