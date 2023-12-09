using MediatR;
using MovieTracker.Infrastructure.Interfaces;

namespace MovieTracker.Features.Movies;

public record DeleteMovieCommand(int Id) : IRequest<Unit>;
public class DeleteMovieHandler : IRequestHandler<DeleteMovieCommand, Unit>
{
    private readonly IMovieService _movieService;

    public DeleteMovieHandler(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public async Task<Unit> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
    {
        await _movieService.DeleteMovieAsync(request.Id);
        return Unit.Value;
    }
}