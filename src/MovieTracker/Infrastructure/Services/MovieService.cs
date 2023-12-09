using AutoMapper;
using MovieTracker.Infrastructure.Interfaces;
using MovieTracker.Models;
using MovieTracker.Models.Dto;
using MovieTracker.Models.ViewModels;

namespace MovieTracker.Infrastructure.Services;

public class MovieService : IMovieService
{
    private readonly IMoviesRepository _moviesRepository;
    private readonly IMapper _mapper;

    public MovieService(IMoviesRepository moviesRepository, IMapper mapper)
    {
        _moviesRepository = moviesRepository;
        _mapper = mapper;
    }

    public async Task<MovieVm> GetMovieAsync(int id)
    {
        var movie = await _moviesRepository.GetByIdAsync(id);
        return movie == null ? null : _mapper.Map<MovieVm>(movie);
    }

    public async Task<IEnumerable<MovieVm>> GetAllMoviesAsync()
    {
        var movies = await _moviesRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<MovieVm>>(movies);
    }

    public async Task<MovieVm> CreateMovieAsync(MovieDto movieDto)
    {
        var movie = _mapper.Map<Movie>(movieDto);
        await _moviesRepository.CreateAsync(movie);
        return _mapper.Map<MovieVm>(movie);
    }

    public async Task UpdateMovieAsync(int id, MovieDto movieDto)
    {
        var movie = await _moviesRepository.GetByIdAsync(id);
        if (movie == null)
        {
            throw new ArgumentException("Movie not found");
        }
        _mapper.Map(movieDto, movie);
        await _moviesRepository.UpdateAsync(movie);
    }

    public async Task DeleteMovieAsync(int id)
    {
        var movie = await _moviesRepository.GetByIdAsync(id);
        if (movie == null)
        {
            throw new ArgumentException("Movie not found");
        }
        await _moviesRepository.DeleteAsync(movie);
    }
    
    public async Task<RatingVm> RateMovieAsync(int movieId, RatingDto ratingDto, string userId)
    {
        var rating = new Rating
        {
            Score = ratingDto.Score,
            MovieId = movieId,
            AppUserId = userId
        };

        await _moviesRepository.AddRatingToMovie(movieId, rating);
        
        var ratingVm = new RatingVm
        {
            Score = ratingDto.Score
        };
        
        return ratingVm;
    }

}