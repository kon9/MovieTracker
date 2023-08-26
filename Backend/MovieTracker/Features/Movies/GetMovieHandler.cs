using AutoMapper;
using MediatR;
using MovieTracker.Infrastructure.Interfaces;
using MovieTracker.Models.ViewModels;

namespace MovieTracker.Features.Movies;

public record GetMovieQuery(int Id) : IRequest<MovieVm>;

public class GetMovieHandler : IRequestHandler<GetMovieQuery, MovieVm>
{
    private readonly IMoviesRepository _movieRepository;
    private readonly IMapper _mapper;

    public GetMovieHandler(IMoviesRepository movieRepository, IMapper mapper)
    {
        _movieRepository = movieRepository;
        _mapper = mapper;
    }

    public async Task<MovieVm> Handle(GetMovieQuery request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetByIdAsync(request.Id);
        return movie == null ? null : _mapper.Map<MovieVm>(movie);
    }
}