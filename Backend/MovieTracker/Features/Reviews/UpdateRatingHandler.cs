using MediatR;
using MovieTracker.Infrastructure.Interfaces;
using MovieTracker.Models;
using MovieTracker.Models.Dto;

namespace MovieTracker.Features.Reviews;

public record UpdateRatingCommand(int Id, RatingDto RatingDto) : IRequest<Unit>;

public class UpdateRatingHandler : IRequestHandler<UpdateRatingCommand, Unit>
{
    private readonly IMoviesRepository _moviesRepository;

    public UpdateRatingHandler(IMoviesRepository moviesRepository)
    {
        _moviesRepository = moviesRepository;
    }

    public async Task<Unit> Handle(UpdateRatingCommand request, CancellationToken cancellationToken)
    {
        var rating = new Rating { Score = request.RatingDto.Score, MovieId = request.Id };
        await _moviesRepository.UpdateRatingForMovie(request.Id, rating);
        return Unit.Value;
    }
}