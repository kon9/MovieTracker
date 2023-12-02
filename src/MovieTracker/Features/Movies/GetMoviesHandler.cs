using AutoMapper;
using MediatR;
using MovieTracker.Infrastructure.Interfaces;
using MovieTracker.Models.ViewModels;

namespace MovieTracker.Features.Movies;

public record GetMoviesQuery() : IRequest<IEnumerable<MovieVm>>;

public class GetMoviesHandler : IRequestHandler<GetMoviesQuery, IEnumerable<MovieVm>>
{
    private readonly IMoviesRepository _movieRepository;
    private readonly IMapper _mapper;

    public GetMoviesHandler(IMoviesRepository movieRepository, IMapper mapper)
    {
        _movieRepository = movieRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MovieVm>> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
    {
        var movies = await _movieRepository.GetAllAsync();

        if (movies == null || !movies.Any())
        {
            return new List<MovieVm>();
        }

        return _mapper.Map<IEnumerable<MovieVm>>(movies);
    }
}