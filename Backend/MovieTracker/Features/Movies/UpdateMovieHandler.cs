using AutoMapper;
using MediatR;
using MovieTracker.Infrastructure.Interfaces;
using MovieTracker.Models;
using MovieTracker.Models.Dto;

namespace MovieTracker.Features.Movies;


public record UpdateMovieCommand(int Id, MovieDto MovieDto) : IRequest<Unit>;
public class UpdateMovieHandler : IRequestHandler<UpdateMovieCommand, Unit>
{
    private readonly IMoviesRepository _moviesRepository;
    private readonly IMapper _mapper;

    public UpdateMovieHandler(IMoviesRepository moviesRepository, IMapper mapper)
    {
        _moviesRepository = moviesRepository;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = _mapper.Map<Movie>(request.MovieDto);
        movie.Id = request.Id;
        await _moviesRepository.UpdateAsync(movie);
        return Unit.Value;
    }
}