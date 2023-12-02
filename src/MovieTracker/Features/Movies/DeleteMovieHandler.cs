using MediatR;
using MovieTracker.Infrastructure.Interfaces;

namespace MovieTracker.Features.Movies;

public record DeleteMovieCommand(int Id) : IRequest<Unit>;
public class DeleteMovieHandler : IRequestHandler<DeleteMovieCommand, Unit>
{
    private readonly IMoviesRepository _moviesRepository;

    public DeleteMovieHandler(IMoviesRepository moviesRepository)
    {
        _moviesRepository = moviesRepository;
    }

    public async Task<Unit> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = await _moviesRepository.GetByIdAsync(request.Id);
        await _moviesRepository.DeleteAsync(movie);
        return Unit.Value;
    }
}