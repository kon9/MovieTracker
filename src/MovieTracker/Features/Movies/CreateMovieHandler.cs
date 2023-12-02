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
    private readonly IMoviesRepository _movieRepository;
    private readonly IMapper _mapper;

    public CreateMovieHandler(IMoviesRepository movieRepository, IMapper mapper)
    {
        _movieRepository = movieRepository;
        _mapper = mapper;
    }

    public async Task<MovieVm> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = _mapper.Map<Movie>(request.MovieDto);
        await _movieRepository.CreateAsync(movie);
        return _mapper.Map<MovieVm>(movie);
    }
}